#include "HashMap.h"

void CommTrans::writeXbee(String command){
  serial.Write(my_ID, 0, command);
}

void CommTrans::proccesCommand(char c){
   
   if( c != '\n' ){
    message += c;
    return
    }
    
   if( message.length() < 5 ) {
    return;
   }

   short send_ID = message[0] - '0';
   short reci_ID = message[0] - '0';

   if( send_ID == 0 ){
    return;
   }

   if( reci_ID == 0 ){
    return;
   }
   
   Void Functoin f = HashMap.get(command);

   if( f == NULL ){
    message == NULL;
    return;
   }

   f();
   
}

void CommTrans::init(){
  
   // set pins, input.output:
  pinMode(CTRL_PIN, OUTPUT);
  digitalWrite(CTRL_PIN, LOW); //set LOW
  pinMode(IR_PIN, INPUT);
  
  //set up serial comms
  Serial.begin(9600);
  Serial.print("+++");
  delay(1500);
  Serial.println("ATID 6968, CH C, CN");
  delay(1100);
  while(Serial.read() != -1){};
}


