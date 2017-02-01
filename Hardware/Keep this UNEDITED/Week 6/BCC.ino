#define buggyControl 3
#define enableXbee 4
#define ledPin  13

#define stopBothMotors 2
#define normalLineFollow 4
//etc

void setup(){
  
  Serial.begin(9600);
  pinMode(ledPin, OUTPUT);
  pinMode(buggyControl, OUTPUT);    // Define output pins
  pinMode(enableXbee, OUTPUT);
  digitalWrite(buggyControl,LOW);
 
  /**************  End of calibration process **********/

  
  Serial.begin(9600);  // Initiate Serial communication
  Serial.print("+++");  // Enter xbee AT command mode, NB no carriage return here
  delay(1500);          // Guard time
  Serial.println("ATID 3333, CH C, CN");


}
void loop(){
  
}

void buggyMode(int mode){

}
