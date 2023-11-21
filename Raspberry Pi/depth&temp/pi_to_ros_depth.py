import rclpy
from rclpy.node import Node
from std_msgs.msg import String
import ms5837
import time

sensor = ms5837.MS5837_30BA() # Default I2C bus is 1 (Raspberry Pi 3)
#sensor = ms5837.MS5837_30BA(0) # Specify I2C bus
#sensor = ms5837.MS5837_02BA()
#sensor = ms5837.MS5837_02BA(0)
#sensor = ms5837.MS5837(model=ms5837.MS5837_MODEL_30BA, bus=0) # Specify model and bus

# We must initialize the sensor before reading it
if not sensor.init():
        print("Sensor could not be initialized")
        exit(1)

# We have to read values from sensor to update pressure and temperature
if not sensor.read():
    print("Sensor read failed!")
    exit(1)

print(("Pressure: %.2f atm  %.2f Torr  %.2f psi") % (
sensor.pressure(ms5837.UNITS_atm),
sensor.pressure(ms5837.UNITS_Torr),
sensor.pressure(ms5837.UNITS_psi)))

print(("Temperature: %.2f C  %.2f F  %.2f K") % (
sensor.temperature(ms5837.UNITS_Centigrade),
sensor.temperature(ms5837.UNITS_Farenheit),
sensor.temperature(ms5837.UNITS_Kelvin)))

freshwaterDepth = sensor.depth() # default is freshwater
sensor.setFluidDensity(ms5837.DENSITY_SALTWATER)
saltwaterDepth = sensor.depth() # No nead to read() again
sensor.setFluidDensity(1000) # kg/m^3
print(("Depth: %.3f m (freshwater)  %.3f m (saltwater)") % (freshwaterDepth, saltwaterDepth))

# fluidDensity doesn't matter for altitude() (always MSL air density)
print(("MSL Relative Altitude: %.2f m") % sensor.altitude()) # relative to Mean Sea Level pressure in air

time.sleep(1)

ref_depth = sensor.depth()

class DepthTempPublisher(Node):
    def __init__(self):
        super().__init__('depthtemp_publisher')
        self.publisher_ = self.create_publisher(String, '/sensor/depthtemp', 50)
        timer_period = 0.03  # seconds
        self.timer = self.create_timer(timer_period, self.timer_callback)

    def timer_callback(self):
        msg = String()
        if sensor.read():
            msg.data = str(sensor.depth()-ref_depth)[:7]
            self.get_logger().info('Publishing: "%s"' % str(msg.data))
        else:
            msg.data = "0"
            self.get_logger().info('Problem with sensor!')

        self.publisher_.publish(msg)
        

def main(args=None):
    rclpy.init(args=args)
    depthtemp_publisher = DepthTempPublisher()
    rclpy.spin(depthtemp_publisher)
    depthtemp_publisher.destroy_node()
    rclpy.shutdown()


if __name__ == '__main__':
    main()
