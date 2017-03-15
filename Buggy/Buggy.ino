#include "CommTrans.h"
#include "Buggy.h"
#include "UltraSonic.h"

CommTrans* comm;
Buggy* buggy;
UltraSonic *sonic;

void setup() {
  MotorControls().stop();
  short buggyID = 1;
  comm = new CommTrans(buggyID);
  comm->init();
  buggy = new Buggy(buggyID, comm);
  comm->setDefaultHandler(   [] { comm->writeXbee("INVALID"); });
  comm->addHandler("PING",   [] { comm->writeXbee("PONG"); });
  comm->addHandler("PONG",   [] { comm->writeXbee("PING"); });
  comm->addHandler("SYN",    [] { });
  comm->addHandler("GO",     [] { buggy->go(); });
  comm->addHandler("STOP",   [] { buggy->stop(); });
  comm->addHandler("PARK",   [] { buggy->park(); });

  attachInterrupt(digitalPinToInterrupt(Buggy::IR_PIN), IR_ISR, RISING);

  sonic = new UltraSonic(buggy, comm);
}

void loop() {
  buggy->update();
  sonic->ultraLoop();
}

void serialEvent() {
  while (Serial.available()) {
    char message = Serial.read();
    comm->processCommand(message);
  }
}

void IR_ISR() {
  buggy->gantry_ISR();
}
