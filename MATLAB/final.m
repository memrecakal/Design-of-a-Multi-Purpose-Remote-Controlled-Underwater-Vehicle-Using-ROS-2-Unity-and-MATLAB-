% Delete all timers from memory.
listOfTimers = timerfindall;
if ~isempty(listOfTimers)
    delete(listOfTimers(:));
end

clear
clc


loop_time = 0.02; %sec

global real_depth;
global unity_desired_depth;
global real_imu;
global unity_desired_imu;
global leftSyringeOccupanc_msg;
global rightSyringeOccupanc_msg;


depthNode = ros2node("/matlab_depth_listener");
desiredDepthNode = ros2node("/matlab_desired_depth_listener");

imuNode = ros2node("/matlab_imu_listener");
desiredImuNode = ros2node("/matlab_desired_imu_listener");

leftSyringeOccupancyNode = ros2node("/matlab_l_syr_talker");
rightSyringeOccupancyNode = ros2node("/matlab_r_syr_talker");

pause(1);

depthSub = ros2subscriber(depthNode,"/sensor/depthtemp");
desiredDepthSub = ros2subscriber(desiredDepthNode,"/unity/depth");
imuSub = ros2subscriber(imuNode, "/sensor/imu/raw");
desiredImuSub = ros2subscriber(desiredImuNode, "/unity/angle");

leftSyringeOccupancPub = ros2publisher(leftSyringeOccupancyNode, ...
    "/matlab/syringe/left", "std_msgs/String");
rightSyringeOccupancPub = ros2publisher(rightSyringeOccupancyNode, ...
    "/matlab/syringe/right", "std_msgs/String");

leftSyringeOccupanc_msg = ros2message(leftSyringeOccupancPub);
rightSyringeOccupanc_msg = ros2message(rightSyringeOccupancPub);

pause(1);

t = timer('Period',loop_time,'ExecutionMode', 'fixedRate');
t.TimerFcn = {@listener_callback, depthSub, desiredDepthSub, desiredImuSub, imuSub};
start(t);

t2 = timer('Period',loop_time,'ExecutionMode', 'fixedRate');
t2.TimerFcn = {@talker_callback, leftSyringeOccupancPub, rightSyringeOccupancPub};
start(t2);


hd=unity_desired_depth; %Desired Depth, m
tetad=unity_desired_imu;  %Desired Angle, deg

dt=0.02; %Time step, s

kd=0.8; %Proportional Gain
ki=0.001; %Integral Gain
kp=0.4; %Derivative Gain

d=0.2; %Diameter of the vehicle, m
m_body=15.51; %Vehicle Mass, kg
L=0.5; %Lenght of the vehicle, m

kd_t=0.2; %Proportional Gain
ki_t=0.001; %Integral Gain
kp_t=0.01; %Derivative Gain
Cd=0.47;     %Drag Coefficient

%Plant
m_tank_1(:,1) = 0.5;         %tank 1 mass
m_tank_2(:,1) = 0.5;         %tank 2 mass
m_tank(:,1) = m_tank_1(:,1)+m_tank_2(:,1);         %Tank Mass, kg
tank_max = 0.2;       %Tank Mass Maximum, kg
V = L*pi*d^2/4;       %Volume, m^3
I = (0.5)*m_body*(d/2)^2;
It= 1/12*m_body*L^2;%inertia
L1 = 0.2;  %Distance of left tank from CM
L2 = 0.2;  %Distance of right tank from CM
rho=997;
g=9.81;

%Positions
h=real_depth; %Initial Depth, m

%Angle of Attack
teta=-real_imu;

%Error, depth
err(:,1) = hd - h;
cum_err(:,1) = 0;
err_rate(:,1)=0;

%Error, angle
err_t(:,1) = tetad- teta;
cum_err_t(:,1) = 0;
err_rate_t(:,1)=0;

%Areas
A_w=pi*d*(0.5)*L;
Afront= pi*(d^2/4);
Ayanal = pi*(d^2/4) + (L-d) * d;

u(:,1)=0;
u_t(:,1)=0;
i=1;


m1_s(:,1)=0;
m2_s(:,1)=0;

a=0;
while a==0

    hd=unity_desired_depth; %Desired Depth, m
    h=real_depth; %Initial Depth, m

    tetad=unity_desired_imu;  %Desired Angle, deg
    teta=-real_imu;


    pause(0.01);


    %%%DEPTH CONTROL UNIT %%%
    err(:,i+1) = hd - h;
    err_rate(:,i+1) = (err(:,i+1) - err(:,i))/dt;
    cum_err(:,i+1) =  cum_err(:,i) + (err(:,i+1))*dt;
    u(:,i+1)=(err(:,i+1))*kp + (err_rate(:,i+1))*kd + (cum_err(:,i+1))*ki;

    %Angle Control Unit
    err_t(:,i+1) = tetad - teta;
    err_rate_t(:,i+1) = (err_t(:,i+1) - err_t(:,i))/dt;
    cum_err_t(:,i+1) =  cum_err_t(:,i) + (err_t(:,i+1))*dt;
    u_t(:,i+1)=(err_t(:,i+1))*kp_t + (err_rate_t(:,i+1))*kd_t + (cum_err_t(:,i+1))*ki_t;

    if u(:,i+1) < -0.05
        u(:,i+1) = -0.05;
    elseif u(:,i+1) > 0.05
        u(:,i+1) = 0.05;
    end
    if u_t(:,i+1) < -0.05
        u_t(:,i+1) = -0.05;
    elseif u_t(:,i+1) > 0.05
        u_t(:,i+1) = 0.05;
    end

    %                  m_tank_1(:,i+1) = m_tank_1(:,i) + u_t(:,i+1);
    %                  m_tank_2(:,i+1) = m_tank_2(:,i) - u_t(:,i+1);

    m_tank_1(:,i+1) = m_tank_1(:,i) + u(:,i+1)+u_t(:,i+1);
    m_tank_2(:,i+1) = m_tank_2(:,i) + u(:,i+1)-u_t(:,i+1);

    if m_tank_1(:,i+1) < 0
        m_tank_1(:,i+1) = 0;
    elseif m_tank_1(:,i+1) > tank_max
        m_tank_1(:,i+1) = tank_max;
    end

    if m_tank_2(:,i+1) < 0             
        m_tank_2(:,i+1) = 0;
    elseif m_tank_2(:,i+1) > tank_max
        m_tank_2(:,i+1) = tank_max;
    end

    m1_s(:,i)=(m_tank_1(:,i+1)/tank_max); %front
    m2_s(:,i)=(m_tank_2(:,i+1)/tank_max);

    leftSyringeOccupanc_msg.data = sprintf('%.6f', m1_s(:,i));
    rightSyringeOccupanc_msg.data = sprintf('%.6f', m2_s(:,i)); %front

    disp(leftSyringeOccupanc_msg.data ...
        + " " + rightSyringeOccupanc_msg.data + " " + real_depth + " " + unity_desired_depth);

    send(leftSyringeOccupancPub, leftSyringeOccupanc_msg);
    send(rightSyringeOccupancPub, rightSyringeOccupanc_msg)

    i=i+1;
    if i == 100000
        %a=1;
    end
end
