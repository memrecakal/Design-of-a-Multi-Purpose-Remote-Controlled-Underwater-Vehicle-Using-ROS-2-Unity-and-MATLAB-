* connection_status: From rosconnector class, takes the connection status and changes text/color indication. 

* sensor_imu_sub: Inherits 'UnitySubscriber<MessageTypes.Std.String>' class. Subs to '/sensor/imu/raw', elects reliable IMU readings and discards others, applies reliable transform data to 3D model on top-left corner.

* slider_controller: Reads/stores desired depth and attack angle values, and reads/shows actual angle value from sensor_imu_sub.

* DepthDesiredPublisher: Inherits 'UnityPublisher<MessageTypes.Std.String>' class. Pubs desired depth value to '/unity/depth'.

* AngkeDesiredPublisher: Inherits 'UnityPublisher<MessageTypes.Std.String>' class. Pubs desired depth value to '/unity/angle'.

* DepthTempSub: Inherits 'UnitySubscriber<MessageTypes.Std.String>' class. Subs to '/sensor/depthtemp' and stores acatual depth value, and just for fun, mimics some random pH values. 

* joystick_handler: Just adds activation mapping for the submarine propellers from the joystick bottom-left.

* JoystickPublisherFinal: Inherits 'UnityPublisher<MessageTypes.Std.String>' class. Reads the mapped actiavtion values from joystick_handler and pubs to '/unity/joystick.' Both 'joystick_handler' and 'JoystickPublisherFinal' are different from original ROS# joystick script.

* Image subsriber: Unchanged, original ROS# script. Subs to '/sensor/camera/compressed' and applies texture to the mesh plane located in center.

* Finally, org. project name if needed: 'unity_ros_new'

    I would like to thank Fenerax Studios for the joystick asset: https://assetstore.unity.com/packages/tools/input-management/joystick-pack-107631 and Siemens for ROS#
