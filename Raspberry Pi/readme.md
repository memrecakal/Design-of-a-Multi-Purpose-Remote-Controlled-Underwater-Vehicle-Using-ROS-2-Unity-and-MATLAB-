* Depth Sensor: 2017 Blue Robotics, https://github.com/bluerobotics/ms5837-python
	- I added pi_to_ros_depthtemp and pi_to_ros_depth, depending on your case, you can choose suiting one but notice the string indexing.
	- Both of them read the sensor(s) and publish to '/sensor/depthtemp' as 'String'
	
* In the 'serial' folder:
	- 'esp_to_serial_imu.py' listens serial (from ESP32) and pubs it to /sensor/imu/raw topic. MIND STRING INDEXING!
	- For debugging purposes, it can also send the ID's of IMU msgs. Yes, ROS msgs already have their IDs but these ones are created by ESP timers, for controller debug.
