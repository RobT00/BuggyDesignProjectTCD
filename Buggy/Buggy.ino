#include "CommTrans.h"
#include "Buggy.h"
#include "UltraSonic.h"

CommTrans* comm;
Buggy* buggy;
UltraSonic *sonic;

void setup() {
  comm = new CommTrans(1);
  comm->init();
  buggy = new Buggy(comm);
  comm->setDefaultHandler( [] (const String & command) { comm->writeXbee("INVALID: " + command); });
  comm->addHandler("PING", [] { comm->writeXbee("PONG"); });
  comm->addHandler("PONG", [] { comm->writeXbee("PING"); });
  comm->addHandler("LED",  [] { buggy->flashLED(); });
  comm->addHandler("GO",   [] { buggy->go(); });
  comm->addHandler("STOP", [] { buggy->stop(); });
  comm->addHandler("PARK", [] { buggy->park(); });

  attachInterrupt(digitalPinToInterrupt(Buggy::IR_PIN), IR_ISR, RISING);

  sonic = new UltraSonic(buggy, comm);
}

void loop() {
  buggy->update();
  sonic->ultraLoop();
}

void serialEvent(){
  while (Serial.available()) {
    char message = Serial.read();
    comm->processCommand(message);
  }
}

void IR_ISR() {
  buggy->gantry_ISR();
}
