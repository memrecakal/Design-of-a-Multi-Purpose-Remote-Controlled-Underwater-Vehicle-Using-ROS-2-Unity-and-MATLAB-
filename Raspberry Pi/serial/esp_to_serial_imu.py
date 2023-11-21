import rclpy
from rclpy.node import Node
from std_msgs.msg import String
import serial

#listens serial and pubs it to /sensor/imu/raw topic

class MinimalPublisher(Node):

    def __init__(self):
        super().__init__('minimal_publisher')
        self.publisher_ = self.create_publisher(String, '/sensor/imu/raw', 50)
        timer_period = 0.03  # seconds
        self.timer = self.create_timer(timer_period, self.timer_callback)

    def timer_callback(self):
        msg = String()
        """ self.serial_data = serial.Serial("/dev/ttyUSB0", 115200).readline(1000) """
        msg.data = str(serial.Serial("/dev/ttyUSB0", 115200).readline())[2:-5]
        #print(str(msg.data))
        self.publisher_.publish(msg)
        self.get_logger().info('Publishing: "%s"' % str(msg.data))

class IDPublisher(Node):

    def __init__(self):
        super().__init__('id_publisher')
        self.publisher_ = self.create_publisher(String, 'sensor/imu/id', 50)
        timer_period = 0.03  # seconds
        self.timer = self.create_timer(timer_period, self.timer_callback)

    def timer_callback(self):
        msg = String()
        """ self.serial_data = serial.Serial("/dev/ttyUSB0", 115200).readline(1000) """
        msg.data = str(serial.Serial("/dev/ttyUSB0", 115200).readline())[2:-5]
        print(str(msg.data))
        self.publisher_.publish(msg)



def main(args=None):
    rclpy.init(args=args)

    minimal_publisher = MinimalPublisher()

    rclpy.spin(minimal_publisher)


    # Destroy the node explicitly
    # (optional - otherwise it will be done automatically
    # when the garbage collector destroys the node object)
    minimal_publisher.destroy_node()
    rclpy.shutdown()


if __name__ == '__main__':
    main()
