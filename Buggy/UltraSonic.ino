int signalPin = 7;
int powerPin = 6;
int groundPin = 5;


bool obstacle; 

unsigned long previousMillis = 0;       

const long interval = 1000;

void setup() {

  Serial.begin(9600);
 
  pinMode(signalPin, OUTPUT);
  pinMode(powerPin, OUTPUT);
  pinMode(groundPin, OUTPUT);
  digitalWrite(groundPin, LOW);
  digitalWrite(powerPin, HIGH);


  
  
}
void loop() {

  unsigned long currentMillis = millis();
  
  if (currentMillis - previousMillis >= interval) {
   
    previousMillis = currentMillis;
  
    unsigned long value = ultraSonic();
  Serial.print(" Value ");
  Serial.println(value);

  if(value < 10){
//    stop(); //stop fxn that is defined elsewhere! Won't move until we say go again.
    //comms->writeXbee("Obstacle");
    Serial.print("OBSTACLE");
    obstacle = true;
  }else if(obstacle == true){
    //comms->writeXbee("PATHCLEAR");
    //go();
    obstacle = false;
    Serial.print("PATHCLEAR");
  }
  
 
  }
}
unsigned long ultraSonic() 
{
  

    unsigned long pulse = 0;
    pinMode(signalPin, OUTPUT);
    digitalWrite(signalPin,LOW);
    digitalWrite(signalPin,HIGH);
    delayMicroseconds(10);
    digitalWrite(signalPin,LOW);

    pinMode(signalPin, INPUT);
    
    pulse = pulseIn(signalPin, HIGH);
    
       double ultrasoundValue = ((pulse/2) * .03432);
       unsigned long ultrasoundValue1;
      ultrasoundValue1 = (unsigned long)ultrasoundValue;
       return ultrasoundValue1;
       
      }
  


