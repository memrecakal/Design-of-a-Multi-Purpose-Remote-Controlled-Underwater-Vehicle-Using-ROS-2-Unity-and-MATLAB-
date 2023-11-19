# Design of a Multi-Purpose Remote Controlled Underwater Vehicle Using ROS/#2 Unity and MATLAB 

https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/48d742b0-af88-4932-8862-06c4480a053f

## Table of Contents  
1. [Introduction](#Introduction)
2. [Communication Framework](#Communication-Framework)
    - [Simulated Scheme: Complete Unity & ROS & MATLAB Control Integration](#simulated-scheme-complete-unity--ros--matlab-control-integration)
      * [Unity + ROS#: Physics & Plant Implementation](#unity--ros-physics--plant-implementation)
      * [MATLAB + ROS2: Control Integration](#matlab--ros2-control-integration)
    - [Application Scheme](#Application-Scheme)
3. [Hardware](#Hardware)
    - [Networking](#Networking)
      * [Android Control Panel](#Android-Control-Panel)
      * [Raspberry Pi & ESP32](#Raspberry-Pi--ESP32)
      * [Latency Analysis](#Latency-Analysis)
    - [Actuators](#Actuators)
    - [Sensors](#sensors)
    - [Overall Electronics Scheme](#Overall-Electronics-Scheme)
4. [Dynamics & Control](#Dynamics--Control)
5. [Design](#design)
6. [CFD Analysis](#CFD-Analysis)
7. [Manufacturing](#manufacturing)

## Introduction

The project aims to create a versatile remote-controlled underwater vehicle with five sensors and a camera to gather real-time environmental data. Utilizing MATLAB, Unity Engine, and ROS# (connected to ROS2), the [hardware](#Hardware) includes an ESP32, Raspberry Pi 4B, wireless router, PC, and Android tablet for remote control. For [simulation](#Communication-Framework), a Unity-based (self-implemented) physics environment simulates the vehicle's dynamics while interfacing with MATLAB (over ROS#) for control. Regarding [application](#Application-Scheme), only the Unity Engine with simulated sensors is replaced with ESP32 and Raspberry, hosting ROS2 nodes for sensor feedback and motor control. In both cases, the (Unity + ROS#) Android app is used for sensor monitoring and controller input for vehicle dynamics. The vehicle boasts 4 DOF facilitated by a ballast mechanism, brushless side motors, and ballast tanks for precise [dynamics](#Dynamics-&-Control). [CFD Analysis](#CFD-Analysis) influenced the [design](#Desing) of the vehicle's impermeable outer shell, ensuring durability in underwater conditions while considering feasible [manufacturing](#manufacturing). 

## Communication Framework
https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/f7dad394-ada4-44de-b159-59335845e236

For this project, sensor feedback is critical for reliable underwater dynamics; even a small latency or error within the transferred IMU readings can result in dangerous motor actuation. So, ROS2 is chosen for its safe and reliable communication capabilities. MATLAB, Unity Engine, and ROS# are used to create relevant design and prototype communication/simulation frameworks. 

* ### Simulated Scheme: Complete Unity & ROS & MATLAB Control Integration
    For simulation, instead of using the real vehicle, ESP32, and Raspberry, a physics environment is modeled within Unity Engine. Rather than using Unity's built-in 'Rigidbody dynamics,' all the relative forces acting upon the vehicle are implemented according to [Dynamics & Control](#Dynamics-&-Control) for feasible communication between simulated sensors (depth and angle of attack) and MATLAB MIMO controller. Input feedback from the Android control panel app is directed to the MATLAB MIMO controller using the ROS MATLAB toolbox; then, proper motor actuation is sent to Unity simulation for the desired depth and angle of attack. Including the wireless router, the remaining system is kept the same. 

    <p align="center" width="100%">
    <img src="https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/bd298083-f39a-47ee-aba1-2502b9cc0714" width="670"> 
        <p align="center">change PID to MIMO + add ros#</p>
    </p>

    - #### Unity + ROS#: Physics & Plant Implementation
    <p align="center" width="100%">
    <img src="https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/4c5ac077-d3c0-4aca-9f2d-831b5bbd0c4b" width="850"> 
    </p>

    The submarine's propulsion system relies on propeller forces determined by propeller positions and power analysis from the manufacturer. Activation data for the propellers is communicated via an Android control panel app through the ROS ecosystem, facilitated by a joystick subscriber script. This information is relayed to Unity Engine, where the "Ros_subs_to_force" class commands the propellers accordingly. Additionally, specialized scripts (as density and center of mass of the vehicle constantly change) like "body_buoyancy" and "body_gravity" control gravity and buoyancy dynamics, allowing the dynamic application of forces on the submarine to manage its underwater movement. 

    - #### MATLAB + ROS2: Control Integration
    <p align="center" width="100%">
    <img src="https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/c3ad0d26-e19b-446f-92e4-664aa39afc0c" width=850> 
    </p>
    
    The depth and angle control system rely on a MIMO controller in MATLAB, requiring inputs of desired depth, desired angle, and sensor feedback for both depth and angle. User-controlled data for desired depth and angle are managed through the Android application, while sensor data originating from the Raspberry Pi in the real-world setup is simulated within Unity Engine for the prototype. Simulated sensor data intentionally introduces delays and noise to mimic real-world conditions. MATLAB's MIMO controller calculates outputs determining the occupancy of syringes, essential for simulating water mass changes in the Unity environment. Additional scripts such as "rearSyringeSub" and "front_syringe_sub" act as intermediaries between Unity and MATLAB, managing the flow of data related to syringe occupancy. Upon receiving this data, Unity employs the "syringeForce" class to translate syringe occupancy into the corresponding force to adjust water mass.

    Additionally, to provide visual feedback to the user, the simulated camera feed needs transmission to the Android controller app. This involves modifying the "CompressedImagePublisher" class (along with the  message structures and headers) of the ROS# library, which originally caters to ROS1, to accommodate ROS2 requirements. Regarding MATLAB's connection with ROS, while MATLAB's ROS Toolbox offers official support for ROS, there's no current support for ROS Bridge, requiring MATLAB and ROS Master to run on the same hardware. Data exchange between ROS and MATLAB necessitates global variables and initializing specific nodes for communication, while additional timers with consistent time steps and corresponding callback functions ensure uninterrupted data updates between ROS and MATLAB without interfering with the MIMO loop.

    <p align="center" width="100%">
    <img src="https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/c19488b8-a2fe-4754-b809-17e60eebbb6d" width="850"> 
    </p>


* ### Application Scheme
    <p align="center" width="100%">
    <img src="https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/b3d55012-8586-4fe9-8cd7-87eadad9ef78" width="1000">
    </p>

    <p align="center" width="100%">
    <img src="https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/53acfc1c-e10e-446e-98a0-2369c7520680" width="1000">
    </p>

    For the prototype, networking hardware is an ESP32, Raspberry Pi 4B (hosting ROS2 nodes), a wireless router (for creating a local network between the ground and underwater), a PC (as ROS master), and an Android tablet as the remote controller (with the Unity for connecting to ROS#). Android app has touchscreen joysticks and sliders for depth, angle, and position control, and all sensors can be monitored through the app. (see [Application Scheme](#Application-Scheme) for details). 

    

## Hardware
* ### Networking
  - #### Android Control Panel
  - #### Raspberry Pi & ESP32
  - #### Latency Analysis
  ![image](https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/946cba39-f76f-4c17-a6f9-a426141222bd)
  ![image](https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/3de87b6a-09c0-4aa0-b3ec-0d607de55c54)
  ![image](https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/89b6d2f2-4b48-4423-804a-ec7ac8bda769)
  ![image](https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/78531717-c70d-4ede-8676-a6d6cf6327a3)
  ![image](https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/5c2669f2-9059-4d0f-972b-94f0c713a8ac)
  ![image](https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/1a11cb74-2f9e-4627-ab96-7acf17e42b28) esp time measurement
  ![image](https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/2d03d7f3-b234-4a0d-8567-ea40d7720d95) slow tablet

* ### Actuators
* ### Sensors
* ### Overal Electronics Scheme





## Dynamics & Control

![image](https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/0f5b07e2-5f33-486d-a381-42d62917e081)

Regarding the vehicle's dynamics, the aim was to create a vehicle with 4 degrees of freedom (DOF) capable of maintaining its position while submerged. Specifically, the vehicle should be capable of linear movement in the Z-axis (horizontal movement) and Y-axis (depth control), as well as rotation in the Y-axis (yaw) and X-axis (pitch). For such movement control, the ballast mechanism has been proposed to change the vehicle's overall density, thereby allowing control of its depth and angle of attack. The side motors facilitate forward movement and right-left turning. Further, coupled brushless motors and ballast tanks are placed symmetrically. The motors provide axial rotation on the Y-axis and linear thrust on the Z-axis, while the ballast tanks provide axial rotation on the X-axis and linear motion on the Y-axis (see [Dynamics & Control](#Dynamics-&-Control) for details). This configuration enables the vehicle to execute sharp and accurate maneuvers, allowing it to operate in unstable conditions and providing operational flexibility in confined spaces. KONTROL EKLE!

## Design
![image](https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/d5ebc5e2-90df-459d-8b28-87b4527c61cd)

![image](https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/d05de9f7-c364-46f6-8143-1773c43d238f)

## CFD Analysis
During the design process, CFD and mechanical structure analysis aided in the design of the vehicle's outer shell, and simulations were used to replicate real-time conditions. Impermeability was taken into account, and the vehicle was designed accordingly to ensure it would withstand underwater conditions (see [Design](#Desing) and [CFD Analysis](#CFD-Analysis) for details).

## Manufacturing


