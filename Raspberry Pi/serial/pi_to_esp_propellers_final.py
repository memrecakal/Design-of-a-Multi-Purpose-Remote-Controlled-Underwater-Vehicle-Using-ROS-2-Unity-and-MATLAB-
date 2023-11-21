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
            '/unity/joystick',
            self.listener_callback,
            10)
        self.subscription  # prevent unused variable warning

    def listener_callback(self, msg):
        for singleData in msg.data.split(" "):

            if singleData[1] == "-":
                self.get_logger().info('I heard: "%s"' % singleData[0:5])
                #ser.write(bytes(msg.data, "utf_8"))
                ser.write(bytes(singleData[1:5]+"\n", "ascii"))
            else:
                self.get_logger().info('I heard: "%s"' % singleData[0:4])
                #ser.write(bytes(msg.data, "utf_8"))
                ser.write(bytes(singleData[1:4]+"\n", "ascii"))



def main(args=None):
    rclpy.init(args=args)
    minimal_subscriber = MinimalSubscriber()
    rclpy.spin(minimal_subscriber)
    minimal_subscriber.destroy_node()
    rclpy.shutdown()

if __name__ == '__main__':
    main()

