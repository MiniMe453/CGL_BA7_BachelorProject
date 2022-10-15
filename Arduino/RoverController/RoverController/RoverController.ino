#include <Encoder.h>

#include<Uduino.h>
Uduino uduino("RoverController");

int buttonInputPins[] = {53, 51, 49, 47, 45, 11};
int numOfDigitalInputs = 6;

int analogInputPins[] = {A0, A1, A2};
int numOfAnalogInputs = 3;

int ledOutputPins[] = {22, 24, 26, 28, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48, 50, 52};

//Rotary Encoder variables
int rotaryAPin = 12;
int rotaryBPin = 13;
int counter = 0;
int aState;
int aLastState;


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

  uduino.addCommand("led", SetLEDPins);
}

void loop() {
    //ReadRotaryEncoder();
  uduino.update();

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

//Serial data structure
/**
0 - testpin1
1 - testPin2
2 - testPin3
3 - testPin4
4 - take photo button
5 - encoder buttons

**/
  uduino.println(serialLine);
  // uduino.delay(10);
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

void ReadRotaryEncoder()
{
  aState = digitalRead(rotaryAPin);

  if(aState != aLastState)
  {
    if(digitalRead(rotaryBPin) != aState)
    {
      counter++;
    } else {
      counter--;
    }
  }

  // if(counter > 512)
  // {
  //   counter = 512;
  // }

  // if(counter < 0)
  // {
  //   counter = 0;
  // }

  aLastState = aState;
}