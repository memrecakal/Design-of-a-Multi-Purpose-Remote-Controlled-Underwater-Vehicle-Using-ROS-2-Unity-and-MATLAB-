function listener_callback(obj, event, depth_message, desired_depth_message, ...
    desired_imu_message, imu_message)

    global real_depth;
    global real_imu;
    global unity_desired_depth;
    global unity_desired_imu;

    rawImuData = strsplit(imu_message.LatestMessage.data);
    last_real_imu = 0;

    rawDepthTempData = strsplit(depth_message.LatestMessage.data);

    real_depth = str2double(rawDepthTempData(1));
    if length(rawImuData) == 3
        real_imu = str2double(rawImuData(3));
        last_real_imu = real_imu;
    else
        real_imu = last_real_imu;
    end

   
    unity_desired_imu = str2double(desired_imu_message.LatestMessage.data);
    unity_desired_depth = str2double(desired_depth_message.LatestMessage.data);
end

