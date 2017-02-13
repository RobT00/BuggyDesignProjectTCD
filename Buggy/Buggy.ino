#include "CommTrans.h"
#include "Buggy.h"

CommTrans comm;
Buggy* buggy;

void setup() {
  comm.init();
  buggy = new Buggy(&comm);
  comm.setDefaultHandler([] { comm.writeXbee("INVALID"); });
  comm.addHandler("PING", [] { comm.writeXbee("PONG"); });
  comm.addHandler("PONG", [] { comm.writeXbee("PING"); });
  comm.addHandler("LED", [] { buggy->flashLED(); });
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
