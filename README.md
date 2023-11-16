# Design of a Multi-Purpose Remote Controlled Underwater Vehicle Using ROS/#2 Unity and MATLAB 



https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/48d742b0-af88-4932-8862-06c4480a053f



https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/c7fa2567-20bf-470b-a517-2e06a2aef68b


## Table of Contents  
[Introduction](#Introduction)  
[Requirements & Constraints](#Requirements-&-Constraints)  
[Design](#design)  
[Dynamics & Control](#dynamicscontrol)  
[CFD Analysis](#cfd)  
[Sensors](#sensors)  
[Overall Communication Scheme](#overallcomm)  
[Simulation](#simulation)  
[Manufacturing](#manuf)  
[Application](#application)  


## Introduction

The goal of this project is to design a versatile underwater vehicle that can be controlled remotely. The primary objective is to create a vehicle with 4 degrees of freedom (DOF) capable of maintaining its position while submerged. Specifically, the vehicle should be capable of linear movement in the Z-axis (horizontal movement), Y-axis (depth control), as well as rotation in the Y-axis (yaw) and X-axis (pitch). For such movement controll, the ballast mechanism has been proposed to change the vehicle's overall density, thereby allowing control of its depth and angle of attack. The side motors facilitate forward movement and right-left turning. Furhter, coupled brushless motors and ballast tanks are placed symmetrically. The motors provide axial rotation in the Y-axis and linear thrust in the Z-axis, while the ballast tanks provide axial rotation in the X-axis and linear motion in the Y-axis (see [Dynamics & Control](#dynamics-&-control)). This configuration enables the vehicle to execute sharp and accurate maneuvers, allowing it to operate in unstable conditions and providing operational flexibility in confined spaces.

The vehicle must gather data from its environment in real-time for feedback control and research purposes. Sensors onboard the vehicle will collect this data, which will be transmitted to the control center for analysis. MATLAB, Unity Engine, ROS# (coupled with ROS2), and, Python are utilized to create relevant communication/simulations framework for design and prototype. During the design process, CFD and mechanical structure analysis aided in the design of the vehicle's outer shell, and simulations were used to replicate real-time conditions. Impermeability was taken into account, and the vehicle was designed accordingly to ensure it would withstand underwater conditions.

The vehicle hosts five sensors for collecting data from its surroundings: pH, temperature, depth, flow rate sensors, and an IMU. Further, a camera is mounted under the body for surveillance and observation. The vehicle can be controlled using an Android application with touchscreen joysticks and sliders for depth, angle, and position control. Further, all sensors can be monitored through the app. A robust and stable communication infrastructure is built using the Robot Operating System (ROS) for such complicated network traffic between underwater and the user.





## Communication Framework
* ### Overall Approach
* ### Simulated Scheme: Complete Unity & ROS MATLAB Control Integration
* ![image](https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/bd298083-f39a-47ee-aba1-2502b9cc0714)

*   - #### Unity + ROS#: Physics & Plant Implementation
    - ![image](https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/4c5ac077-d3c0-4aca-9f2d-831b5bbd0c4b)

*   - #### MATLAB + ROS2: Control Integration

* ### Application Scheme
    <p align="center" width="100%">
    <img src="https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/53acfc1c-e10e-446e-98a0-2369c7520680" width="1000">
    </p>


## Hardware
* ### Networking
  - #### Android Control Panel
  - #### Raspberry Pi /  ESP32
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

## Design
![image](https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/d5ebc5e2-90df-459d-8b28-87b4527c61cd)

![image](https://github.com/memrecakal/Design-of-a-Multi-Purpose-Remote-Controlled-Underwater-Vehicle-Using-ROS-2-Unity-and-MATLAB-/assets/42466646/d05de9f7-c364-46f6-8143-1773c43d238f)

## CFD Analysis

## Manufacturing


