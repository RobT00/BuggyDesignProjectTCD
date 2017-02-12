#include "CommTrans.h"
#include "MotorControls.h"

CommTrans comm;

void setup() {
  comm.init();
  comm.addHandler("PING", [] { comm.writeXbee("PONG"); });
  comm.addHandler("PONG", [] { comm.writeXbee("PING"); });
}

void loop() {
  // put your main code here, to run repeatedly:

}

void serialEvent(){
  while (Serial.available()) {
    char message = Serial.read();
    comm.processCommand(message);
  }
}
