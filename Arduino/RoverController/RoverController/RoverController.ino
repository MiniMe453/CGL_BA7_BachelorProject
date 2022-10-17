#include<Uduino.h>
#include<LiquidCrystal.h>


Uduino uduino("RoverController");
LiquidCrystal lcd(10,9,5,6,7,8);

int buttonInputPins[] = {53, 51, 49, 47, 45, 11, 41, 39,37};
int numOfDigitalInputs = 9;

int analogInputPins[] = {A0, A1, A2};
int numOfAnalogInputs = 3;

int ledOutputPins[] = {22, 24, 26, 28, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48, 50, 52};

//Rotary Encoder variables
int rotaryAPin = 12;
int rotaryBPin = 13;
int counter = 0;
bool interruptCalled = false;
unsigned long timeSinceLastInterrupt;
unsigned long timeSinceLastMessage;
unsigned long interruptResetDelay = 25;
unsigned long messageDelay = 10;

int aState;
int aLastState;

int aPinCounter;
int aPinCurrentState= LOW;
int aPinReading;
int bPinCounter;
int bPinCurrentState = LOW;
int bPinReading;
int debounce_count = 2;


void setup() {
  Serial.begin(9600);

  for(int i = 0;i<numOfDigitalInputs;i++)
  {
    pinMode(buttonInputPins[i], INPUT_PULLUP);
  }

  for(int i = 0;i<16;i++)
  {
    pinMode(ledOutputPins[i], OUTPUT);
  }

  pinMode(rotaryAPin, INPUT);
  pinMode(rotaryBPin, INPUT);
  lcd.begin(16,2);

  uduino.addCommand("led", SetLEDPins);
  uduino.addCommand("lcd", UpdateLCD);
}

void loop() {
  uduino.update();

  //This makes it accurate. DO NOT DELETE
  ReadEncoders();
  SoftwareDebouncer(rotaryAPin, aPinReading, aPinCurrentState, aPinCounter);
  SoftwareDebouncer(rotaryBPin, bPinReading, bPinCurrentState, bPinCounter);

  if((aPinCurrentState == 0 || bPinCurrentState == 0) && !interruptCalled)
  {
    interruptCalled = true;
    ReadEncoders();
    timeSinceLastInterrupt = millis();
  }

  //Serial.println(counter);

  String serialLine = "_";

  for(int i = 0;i<numOfDigitalInputs;i++)
  {
    serialLine += String(digitalRead(buttonInputPins[i]));
  }

  for(int i = 0;i<numOfAnalogInputs;i++)
  {
    serialLine += " ";
    serialLine += String(analogRead(analogInputPins[i]));
  }

  serialLine += " ";
  serialLine += String(counter);

//Serial data structure
/**
0 - testpin1
1 - testPin2
2 - testPin3
3 - testPin4
4 - take photo button
5 - encoder buttons

**/

  if(millis() - timeSinceLastInterrupt > interruptResetDelay && interruptCalled)
  {
    interruptCalled = false;
  }

  if(millis() - timeSinceLastMessage > messageDelay)
  {
      uduino.println(serialLine);
      timeSinceLastMessage = millis();
  }
  //Serial.println(counter);
  //uduino.delay(5);

  //Serial.println(serialLine);

  //uduino.println(counter);
}

void SetLEDPins()
{
  int numOfParams = uduino.getNumberOfParameters();
  char *arg;
  arg = uduino.next();

  if(numOfParams > 0)
  {
    for(int i = 0;i<numOfParams;i++)
    {
      if(arg != NULL)
        digitalWrite(ledOutputPins[i], uduino.charToInt(arg));

      arg = uduino.next();
    }
  }
}

void UpdateLCD()
{
  char *arg;
  arg = uduino.next();

  if(arg != NULL)
  {
    lcd.setCursor(0, 0);
    lcd.print(arg);
    arg = uduino.next();
    lcd.setCursor(0,1);
    lcd.print(arg);
  }  
}

void ReadEncoders()//bool readAState, bool secondPin
{
  aState = digitalRead(rotaryAPin);
   // If the previous and the current state of the outputA are different, that means a Pulse has occured
   if (aState != aLastState){     
     // If the outputB state is different to the outputA state, that means the encoder is rotating clockwise
     if (digitalRead(rotaryBPin) != aState) { 
       if(counter + 1 <= 512)
        counter ++;
     } else {
       if(counter - 1 >= 0)
        counter --;
     }
   } 
   aLastState = aState; // Updates the previous state of the outputA with the current state
}

int SoftwareDebouncer(int pin, int& pinReading, int& pinCurrentState, int& pinCounter)
{
  bool inLoop = true;
  int loopCounter = 0;

  while(inLoop && loopCounter < 5)
  {
    loopCounter++;
    pinReading = digitalRead(pin);
    if(pinReading == pinCurrentState && pinCounter > 0)
    {
      pinCounter--;
    }

    if(pinReading != pinCurrentState)
    {
      pinCounter++;
    }

    if(pinCounter >= debounce_count)
    {
      pinCounter = 0;
      pinCurrentState = pinReading;
      return pinCurrentState;
    }
  }

  return -1;
}