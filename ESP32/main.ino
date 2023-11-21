
#include "I2Cdev.h"

#include "MPU6050_6Axis_MotionApps20.h"

#if I2CDEV_IMPLEMENTATION == I2CDEV_ARDUINO_WIRE
#include "Wire.h"
#endif

MPU6050 mpu;

#define OUTPUT_READABLE_YAWPITCHROLL

#define INTERRUPT_PIN 2  // use pin 2 on Arduino Uno & most boards
#define LED_PIN 13 // (Arduino is 13, Teensy is 11, Teensy++ is 6)
bool blinkState = false;

// MPU control/status vars
bool dmpReady = false;  // set true if DMP init was successful
uint8_t mpuIntStatus;   // holds actual interrupt status byte from MPU
uint8_t devStatus;      // return status after each device operation (0 = success, !0 = error)
uint16_t packetSize;    // expected DMP packet size (default is 42 bytes)
uint16_t fifoCount;     // count of all bytes currently in FIFO
uint8_t fifoBuffer[64]; // FIFO storage buffer

// orientation/motion vars
Quaternion q;           // [w, x, y, z]         quaternion container
VectorInt16 aa;         // [x, y, z]            accel sensor measurements
VectorInt16 aaReal;     // [x, y, z]            gravity-free accel sensor measurements
VectorInt16 aaWorld;    // [x, y, z]            world-frame accel sensor measurements
VectorFloat gravity;    // [x, y, z]            gravity vector
float euler[3];         // [psi, theta, phi]    Euler angle container
float ypr[3];           // [yaw, pitch, roll]   yaw/pitch/roll container and gravity vector

// packet structure for InvenSense teapot demo
uint8_t teapotPacket[14] = { '$', 0x02, 0, 0, 0, 0, 0, 0, 0, 0, 0x00, 0x00, '\r', '\n' };



// ================================================================
// ===               INTERRUPT DETECTION ROUTINE                ===
// ================================================================

volatile bool mpuInterrupt = false;     // indicates whether MPU interrupt pin has gone high
void dmpDataReady() {
  mpuInterrupt = true;
}

// ================================================================
// ===                    STEPPER VARIABLES                     ===
// ================================================================

const int frontStepDirPin = 32; //32
const int frontStepPin = 33;  //33
const int rearStepDirPin = 26;//26
const int rearStepPin = 35; //35

int frontCurrentTurn = 0;
int frontDesiredTurn = 0;
int rearCurrentTurn = 0;
int rearDesiredTurn = 0;
const int stepLimit = 1200;

unsigned long currentMicrons = 0;
unsigned long prevMicrons = 0;
const float oneStepAngle = 1.8;
const int stepsForOneTurn = (int) 360 / oneStepAngle;

int frontStepStat = LOW;
int rearStepStat = LOW;

int frontStepDir = HIGH;
int rearStepDir = HIGH;

bool frontNeedsToTurn = false;
bool rearNeedsToTurn = false;

const int stepperFreq = 1000;
const int stepperRes = 8;
int frontStepperDuty = 127;
int rearStepperDuty = 127;
const int frontStepChannel = 2;
const int rearStepChannel = 3;

//TODO: REMOVE
const int potPin = 34;
int potVal = 0;

// ================================================================
// ===                    BRUSHLESS VARIABLES                   ===
// ================================================================

const int leftMotorPin = 25; 
const int rightMotorPin = 27; 
float leftMotorDuty = 0;
float rightMotorDuty = 0;

// setting PWM properties
const int motorFreq = 50;
const int motorResolution = 10;

const int leftMotorChannel = 0;
const int rightMotorChannel = 1;


// ================================================================
// ===                           TIMER VARIABLES                ===
// ================================================================

uint32_t gyroSendTime;
uint32_t matlabInputTime;
uint32_t gyroID = 0;

// ================================================================
// ===                     ROS SUB VARIABLES                    ===
// ================================================================

int matlabMotor = 0;

// ================================================================
// ===                    SERIAL UTILS                          ===
// ================================================================


// how much serial data we expect before a newline
const unsigned int m_input = 50;

String rawData;
int msgID;
int Data;

// here to process incoming serial data after a terminator received
void process_data (const char * data) {
  rawData = data;
  

  //int index = rawData.indexOf(" ");
  int index = 1;
  //matlabMotorID = rawData.toInt();
  msgID = rawData.substring(0, index).toInt();

  if (rawData.substring(index, index+1) == "-") {
    Data = -rawData.substring(index+1, rawData.length()).toInt();
    }
  else {
    Data = rawData.substring(index, rawData.length()).toInt();
    }

  /*
  Serial.print("Data: ");
  Serial.print(Data);
  Serial.print(" left: ");
  Serial.print(leftMotorDuty);
  Serial.print(" right: ");
  Serial.println(rightMotorDuty);
  */


}  // end of process_data

void processIncomingByte (const byte inByte)
{
  static char input_line [m_input];
  static unsigned int input_pos = 0;

  switch (inByte)
  {

    case '\n':   // end of text
      input_line [input_pos] = 0;  // terminating null byte

      // terminator reached! process input_line here ...
      process_data (input_line);

      // reset buffer for next time
      input_pos = 0;
      break;

    case '\r':   // discard carriage return
      break;

    default:
      // keep adding if not full ... allow for terminating null byte
      if (input_pos < (m_input - 1))
        input_line [input_pos++] = inByte;
      break;

  }  // end of switch

} // end of processIncomingByte

// ================================================================
// ===                      INITIAL SETUP                       ===
// ================================================================

void setup() {
  // join I2C bus (I2Cdev library doesn't do this automatically)
#if I2CDEV_IMPLEMENTATION == I2CDEV_ARDUINO_WIRE
  Wire.begin();
  Wire.setClock(400000); // 400kHz I2C clock. Comment this line if having compilation difficulties
#elif I2CDEV_IMPLEMENTATION == I2CDEV_BUILTIN_FASTWIRE
  Fastwire::setup(400, true);
#endif

  // initialize serial communication
  // (115200 chosen because it is required for Teapot Demo output, but it's
  // really up to you depending on your project)
  Serial.begin(115200);
  while (!Serial); // wait for Leonardo enumeration, others continue immediately

  // NOTE: 8MHz or slower host processors, like the Teensy @ 3.3V or Arduino
  // Pro Mini running at 3.3V, cannot handle this baud rate reliably due to
  // the baud timing being too misaligned with processor ticks. You must use
  // 38400 or slower in these cases, or use some kind of external separate
  // crystal solution for the UART timer.

  // initialize device
  Serial.println(F("Initializing I2C devices..."));
  mpu.initialize();
  pinMode(INTERRUPT_PIN, INPUT);

  // verify connection
  Serial.println(F("Testing device connections..."));
  Serial.println(mpu.testConnection() ? F("MPU6050 connection successful") : F("MPU6050 connection failed"));


  // load and configure the DMP
  Serial.println(F("Initializing DMP..."));
  devStatus = mpu.dmpInitialize();

  // supply your own gyro offsets here, scaled for min sensitivity
  mpu.setXGyroOffset(220);
  mpu.setYGyroOffset(76);
  mpu.setZGyroOffset(-85);
  mpu.setZAccelOffset(1788); // 1688 factory default for my test chip

  // make sure it worked (returns 0 if so)
  if (devStatus == 0) {
    // Calibration Time: generate offsets and calibrate our MPU6050
    mpu.CalibrateAccel(6);
    mpu.CalibrateGyro(6);
    mpu.PrintActiveOffsets();
    // turn on the DMP, now that it's ready
    Serial.println(F("Enabling DMP..."));
    mpu.setDMPEnabled(true);

    // enable Arduino interrupt detection
    Serial.print(F("Enabling interrupt detection (Arduino external interrupt "));
    Serial.print(digitalPinToInterrupt(INTERRUPT_PIN));
    Serial.println(F(")..."));
    attachInterrupt(digitalPinToInterrupt(INTERRUPT_PIN), dmpDataReady, RISING);
    mpuIntStatus = mpu.getIntStatus();

    // set our DMP Ready flag so the main loop() function knows it's okay to use it
    Serial.println(F("DMP ready! Waiting for first interrupt..."));
    dmpReady = true;

    // get expected DMP packet size for later comparison
    packetSize = mpu.dmpGetFIFOPacketSize();
  } else {
    // ERROR!
    // 1 = initial memory load failed
    // 2 = DMP configuration updates failed
    // (if it's going to break, usually the code will be 1)
    Serial.print(F("DMP Initialization failed (code "));
    Serial.print(devStatus);
    Serial.println(F(")"));
  }

  // configure LED for output
  pinMode(LED_PIN, OUTPUT);


  // Stepper Init
  
  pinMode(frontStepPin, OUTPUT);
  pinMode(frontStepDirPin, OUTPUT);

  pinMode(rearStepPin, OUTPUT);
  pinMode(rearStepDirPin, OUTPUT);
  
  ledcSetup(frontStepChannel, stepperFreq, stepperRes);
  ledcAttachPin(frontStepPin, frontStepChannel);

  ledcSetup(rearStepChannel, stepperFreq, stepperRes);
  ledcAttachPin(rearStepPin, rearStepChannel);
  
  // Brushless Init
  ledcSetup(leftMotorChannel, motorFreq, motorResolution);
  ledcAttachPin(leftMotorPin, leftMotorChannel);

  ledcSetup(rightMotorChannel, motorFreq, motorResolution);
  ledcAttachPin(rightMotorPin, rightMotorChannel);
}



// ================================================================
// ===                    MAIN PROGRAM LOOP                     ===
// ================================================================

void loop() {
// if programming failed, don't try to do anything
  //if (!dmpReady) return;

  // Serial input for motors
  while (Serial.available () > 0) {
    processIncomingByte(Serial.read());
    matlabInputTime = millis();

    switch (msgID) {
      Serial.println(Data);
      case 0: 
        leftMotorDuty = (1024/20) * (1.5 + 0.514 * Data/100.0);
        break;
      
      case 1: 
        rightMotorDuty = (1024/20) * (1.5 + 0.514 * Data/100.0);
        break;
      
      case 2:
        //frontDesiredTurn = map(potVal, 0, 4095, 0, stepLimit);
        break;
      
      case 3:
        //rearDesiredTurn = stepLimit - frontDesiredTurn;
        break;
      
  }
  }

  potVal = analogRead(potPin);

  frontDesiredTurn = map(potVal, 0, 4095, 0, stepLimit);
  rearDesiredTurn = stepLimit - frontDesiredTurn;

/*
  Serial.print("frontDesired: ");
  Serial.print(frontDesiredTurn);
  Serial.print(" frontcurrent: ");
  Serial.print(frontCurrentTurn);
  Serial.print(" rearDesired: ");
  Serial.print(rearDesiredTurn);
  Serial.print(" rearcurrent: ");
  Serial.println(rearCurrentTurn);
  */
  
  // Stepper activation
  currentMicrons = micros();
 
  
  // Front stepper
  frontStepperDuty = 127;
  if (frontDesiredTurn - frontCurrentTurn > 10.0) {
    
    frontStepDir = HIGH;
    digitalWrite(frontStepDirPin, HIGH);
    
    if (currentMicrons - prevMicrons >= 1000 * stepsForOneTurn) { // in every 1000ms, 1.8deg turn
      frontCurrentTurn += 1;
      prevMicrons = currentMicrons;
    } 
  }

  else if (frontCurrentTurn - frontDesiredTurn > 10.0) {
   
    frontStepDir = LOW;
    digitalWrite(frontStepDirPin, LOW);

    
    if (currentMicrons - prevMicrons >= 1000 * stepsForOneTurn) { // in every 1ms, 1.8deg turn
      frontCurrentTurn -= 1;
      prevMicrons = currentMicrons;
    } 
  }

  else {
    frontStepperDuty = 0;
    }

  // Rear Stepper
  rearStepperDuty = 127;
  if (rearDesiredTurn - rearCurrentTurn > 10.0) {
    
    rearStepDir = HIGH;
    digitalWrite(rearStepDirPin, HIGH);
    
    if (currentMicrons - prevMicrons >= 1000 * stepsForOneTurn) { // in every 1000ms, 1.8deg turn
      rearCurrentTurn += 1;
      prevMicrons = currentMicrons;
    } 
  }

  else if (rearCurrentTurn - rearDesiredTurn > 10.0) {
   
    rearStepDir = LOW;
    digitalWrite(rearStepDirPin, LOW);

    
    if (currentMicrons - prevMicrons >= 1000 * stepsForOneTurn) { // in every 1ms, 1.8deg turn
      rearCurrentTurn -= 1;
      prevMicrons = currentMicrons;
    } 
  }

  else {
    rearStepperDuty = 0;
    }

    digitalWrite(frontStepDirPin, frontStepDir);
    ledcWrite(frontStepChannel, frontStepperDuty);

    digitalWrite(rearStepDirPin, rearStepDir);
    ledcWrite(rearStepChannel, rearStepperDuty);

  // Brushless activation
  ledcWrite(leftMotorChannel, leftMotorDuty);
  ledcWrite(rightMotorChannel, rightMotorDuty);



  // read a packet from FIFO
  if (mpu.dmpGetCurrentFIFOPacket(fifoBuffer)) { // Get the Latest packet
#ifdef OUTPUT_READABLE_YAWPITCHROLL
    // display Euler angles in degrees
    mpu.dmpGetQuaternion(&q, fifoBuffer);
    mpu.dmpGetGravity(&gravity, &q);
    mpu.dmpGetYawPitchRoll(ypr, &q, &gravity);


    
    Serial.print(ypr[0] * 180 / M_PI);
    Serial.print(" ");
    Serial.print(ypr[1] * 180 / M_PI);
    Serial.print(" ");
    Serial.print(ypr[2] * 180 / M_PI);
    
    //delay(2);

    Serial.println();
    
    
    // blink LED to indicate activity
    blinkState = !blinkState;
  }


  
  

#endif


} // end of loop()
