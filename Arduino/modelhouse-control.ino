#include <Servo.h>
#include <dht.h>

Servo myservo;
dht DHT1;
dht DHT2;

const int firstDHT = 7; 
const int secondDHT = 6;
const int LED1 = 13;
const int LED2 = 12;
const int LED3 = 8;
const int LED4 = 4;
const int FAN = 3;
const int linear_A = 5;
const int RADIATOR = 11;

bool radiator_AUTO = false;
bool window_AUTO = false;
bool fan_AUTO = false;
bool ON = false;
bool go_OFF = false;
bool kickstart = false;

int TEMP1;
int TEMP2;
int SPEED;
int under;
int over;
int HUMI1;
int HUMI2;
int wOPEN;
int HEAT;

char c;
String SerialData;

void pinmodes() {
  //LIGHTS
  pinMode(LED1, OUTPUT);    digitalWrite(LED1, LOW);
  pinMode(LED2, OUTPUT);    digitalWrite(LED2, LOW);
  pinMode(LED3, OUTPUT);    digitalWrite(LED3, LOW);
  pinMode(LED4, OUTPUT);    digitalWrite(LED4, LOW);

  //FAN
  pinMode(FAN, OUTPUT);     digitalWrite(FAN, LOW);

  //RADIATOR
  pinMode(RADIATOR, OUTPUT); digitalWrite(RADIATOR, LOW);
  
  //TEMP AND HUMI
  pinMode(firstDHT, INPUT);
  pinMode(secondDHT, INPUT);

 
}

void setup() {
  Serial.begin(9600);
  pinmodes();
  myservo.attach(linear_A);
  myservo.writeMicroseconds(1000);
}

void loop() {
  DHT1.read22(firstDHT);
  TEMP1 = DHT1.temperature;
  HUMI1 = DHT1.humidity;
  Serial.print(TEMP1); Serial.println("-T1#");
  Serial.print(HUMI1); Serial.println("-H1#");
  
  DHT2.read22(secondDHT);
  TEMP2 = DHT2.temperature;
  HUMI2 = DHT2.humidity;
  Serial.print(TEMP2); Serial.println("-T2#");
  Serial.print(HUMI2); Serial.println("-H2#");
  delay(450);
  
  int sensorVal = analogRead(A0);
  Serial.println(sensorVal);
  
  while (Serial.available() > 0)
  {
    c = Serial.read();
    SerialData += c;
  }
  
  if (c == '#')
  {
    //LIGHTS
    if (SerialData == "1ON#") {
      digitalWrite(LED1, HIGH);
    }
    else if (SerialData == "1OFF#") {
      digitalWrite(LED1, LOW);
    }

    if (SerialData == "2ON#") {
      digitalWrite(LED2, HIGH);
    }
    else if (SerialData == "2OFF#") {
      digitalWrite(LED2, LOW);
    }

    if (SerialData == "3ON#") {
      digitalWrite(LED3, HIGH);
    }
    else if (SerialData == "3OFF#") {
      digitalWrite(LED3, LOW);
    }

    if (SerialData == "4ON#") {
      digitalWrite(LED4, HIGH);
    }
    else if (SerialData == "4OFF#") {
      digitalWrite(LED4, LOW);
    }

    if (SerialData == "allON#") {
      digitalWrite(LED1, HIGH);
      digitalWrite(LED2, HIGH);
      digitalWrite(LED3, HIGH);
      digitalWrite(LED4, HIGH);
    }

    else if (SerialData == "allOFF#") {
      digitalWrite(LED1, LOW);
      digitalWrite(LED2, LOW);
      digitalWrite(LED3, LOW);
      digitalWrite(LED4, LOW);
    }

    //FAN
    if (SerialData == "FMOff#") {
      analogWrite(FAN, 0);
      PrintFanSpeed(0);
    }
    else if (SerialData == "FMLow#") {
      analogWrite(FAN, 200);
      delay(300);
      analogWrite(FAN, 85);
      PrintFanSpeed(85);
    }
    else if (SerialData == "FMMed#") {
      analogWrite(FAN, 200);
      delay(300);
      analogWrite(FAN, 170);
      PrintFanSpeed(170);
    }
    else if (SerialData == "FMHigh#") {
      analogWrite(FAN, 255);
      PrintFanSpeed(255);
    }

    if (SerialData == "FAOn#") {
      kickstart = true;
      fan_AUTO = true;
    }
    else if (SerialData == "FAOff#") {
      analogWrite(FAN, 0);
      fan_AUTO = false;
      PrintFanSpeed(0);
    }

    //RADIATOR
    if (SerialData == "RMOff#") {
      analogWrite(RADIATOR, 0);
      PrintRadiatorHeat(0);
    }
    else if (SerialData == "RMLow#") {
      analogWrite(RADIATOR, 85);
      PrintRadiatorHeat(85);
    }
    else if (SerialData == "RMMed#") {
      analogWrite(RADIATOR, 170);
      PrintRadiatorHeat(170);
    }
    else if (SerialData == "RMHigh#") {
      analogWrite(RADIATOR, 255);
      PrintRadiatorHeat(255);
    }

    if (SerialData == "RAOn#") {
      radiator_AUTO = true;
    }
    else if (SerialData == "RAOff#") {
      analogWrite(RADIATOR, 0);
      radiator_AUTO = false;
      PrintRadiatorHeat(0);
    }
    
    //ALARM    
    if (SerialData == "aON#") {
      under = sensorVal - 200;
      over = sensorVal + 200;
      ON = true;
    }
    
    if (SerialData == "aOFF#") {
      if (go_OFF == true) {
        tone(2, 3000, 1000);
      }
      ON = false;
      go_OFF = false;
    }

    //WINDOW
    if (SerialData == "WMClose#") {
      myservo.writeMicroseconds(1000);
      PrintWindowOpen(1000);
    }
    else if (SerialData == "WMHalf#") {
      myservo.writeMicroseconds(1500);
      PrintWindowOpen(1500);
    }
    else if (SerialData == "WMFull#") {
      myservo.writeMicroseconds(2000); 
      PrintWindowOpen(2000);
    }

    if (SerialData == "WAOn#") {
      window_AUTO = true;
    }
    else if (SerialData == "WAOff#") {
      window_AUTO = false;
      myservo.writeMicroseconds(1000);
      PrintWindowOpen(1000);
    }
    
    c = 0;
    SerialData = "";
  } 
  
  //FAN
  if (fan_AUTO == true) {
    
    if(TEMP1 > 30){ TEMP1 = 30; }

    SPEED = map(TEMP1, 22, 30, 100, 255);
 
    if (TEMP1 < 22){ SPEED = 0; }
    

    PrintFanSpeed(SPEED);
    
    if(kickstart == true){
      analogWrite(FAN, 255);
      kickstart = false;
    }
    
    analogWrite(FAN, SPEED);
  }

  
  //RADIATOR
  if (radiator_AUTO == true) {
  
    if(TEMP1 > 22){ TEMP1 = 22; }

    HEAT = map(TEMP1, 15, 22, 255, 0);
 
    if (TEMP1 < 15){ HEAT = 255; }
 
    PrintRadiatorHeat(HEAT);
    
    analogWrite(RADIATOR, HEAT);
  }

  //ALARM
  if (ON == true) {
    if (sensorVal < under || sensorVal > over) {
      go_OFF = true;
    }
  }

  if (go_OFF == true) {
    tone(2, 3000);
    Serial.print("112-A1#");
  }

  //WINDOW
  if (window_AUTO == true) {
  
    if(HUMI2 > 80){ HUMI2 = 80; }

    wOPEN = map(HUMI2, 28, 80, 1000, 2000);
 
    if (HUMI2 < 28){ wOPEN = 1000; }

    PrintWindowOpen(wOPEN);
 
    myservo.writeMicroseconds(wOPEN);
  }
}

void PrintFanSpeed(int S){
  int fan_PB = map(S, 0, 255, 0, 100);
  Serial.print(fan_PB); Serial.println("-S1#");
}

void PrintRadiatorHeat(int H){
  int radiator_PB = map(H, 0, 255, 0, 100);
  Serial.print(radiator_PB); Serial.println("-R1#");
}

void PrintWindowOpen (int O) {
  int window_PB = map(O, 1000, 2000, 0, 100);
  Serial.print(window_PB); Serial.println("-W1#");
}
