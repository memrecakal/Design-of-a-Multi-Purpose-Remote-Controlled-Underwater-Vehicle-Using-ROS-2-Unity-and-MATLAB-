from encodings import utf_8
import rclpy
from rclpy.node import Node
from std_msgs.msg import String
import serial

# listens /unity/joystick/left and /unity/joystick/right topic and writes serial 

ser = serial.Serial("/dev/ttyUSB0", 115200)

class MinimalSubscriber(Node):

    def __init__(self):
        super().__init__('minimal_subscriber')
        self.subscription = self.create_subscription(
            String,
            '/unity/joystick/left',
            self.listener_callback,
            10)
        self.subscription  # prevent unused variable warning

    def listener_callback(self, msg):
        if msg.data[2] == "-":
            self.get_logger().info('I heard: "%s"' % msg.data[0:5])
            #ser.write(bytes(msg.data, "utf_8"))
            ser.write(bytes(msg.data[1:5]+"\n", "ascii"))
        else:
            self.get_logger().info('I heard: "%s"' % msg.data[0:4])
            #ser.write(bytes(msg.data, "utf_8"))
            ser.write(bytes(msg.data[1:4]+"\n", "ascii"))

class RightSubscriber(Node):

    def __init__(self):
        super().__init__('right_subscriber')
        self.subscription = self.create_subscription(
            String,
            '/unity/joystick/right',
            self.listener_callback,
            10)
        self.subscription  # prevent unused variable warning

    def listener_callback(self, msg):
        if msg.data[2] == "-":
            self.get_logger().info('I heard: "%s"' % msg.data[0:5])
            #ser.write(bytes(msg.data, "utf_8"))
            ser.write(bytes(msg.data[1:5]+"\n", "ascii"))
        else:
            self.get_logger().info('I heard: "%s"' % msg.data[0:4])
            #ser.write(bytes(msg.data, "utf_8"))
            ser.write(bytes(msg.data[1:4]+"\n", "ascii"))


def main(args=None):
    rclpy.init(args=args)
    minimal_subscriber = MinimalSubscriber()
    right_subscriber = RightSubscriber()
    rclpy.spin(minimal_subscriber)
    rclpy.spin(right_subscriber)
    minimal_subscriber.destroy_node()
    right_subscriber.destroy_node()
    rclpy.shutdown()

if __name__ == '__main__':
    main()

