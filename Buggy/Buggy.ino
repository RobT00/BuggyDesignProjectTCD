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
  comm.addHandler("GO", [] { buggy->go(); });
  comm.addHandler("STOP", [] { buggy->stop(); });
  comm.addHandler("PARK", [] { buggy->park(); });

  attachInterrupt(digitalPinToInterrupt(Buggy::IR_PIN), IR_ISR, RISING);
}

void loop() {
  buggy->update();
}

void serialEvent(){
  while (Serial.available()) {
    char message = Serial.read();
    comm.processCommand(message);
  }
}

void IR_ISR() {
  buggy->gantry_ISR();
}
