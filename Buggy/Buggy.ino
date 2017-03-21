#include "CommTrans.h"
#include "Buggy.h"
#include "UltraSonic.h"
#include "Lights.h"

CommTrans *comm;
MotorControls *motors;
Buggy *buggy;
UltraSonic *sonic;
Lights *christmasTree;

void setup() {
  motors = new MotorControls();
  christmasTree = new Lights();
  christmasTree->setMotor(motors);

  short buggyID = 1;
  comm = new CommTrans(buggyID);
  comm->init();
  buggy = new Buggy(buggyID, motors, comm);
  comm->setDefaultHandler(   [] { comm->writeXbee("INVALID"); });
  comm->addHandler("PING",   [] { comm->writeXbee("PONG"); });
  comm->addHandler("PONG",   [] { comm->writeXbee("PING"); });
  comm->addHandler("SYN",    [] { });
  comm->addHandler("GO",     [] { buggy->go(); });
  comm->addHandler("STOP",   [] { buggy->stop(); });
  comm->addHandler("PARK",   [] { buggy->park(); });

  attachInterrupt(digitalPinToInterrupt(Buggy::IR_PIN), IR_ISR, RISING);

  sonic = new UltraSonic(buggy, comm);
  christmasTree->setUltrasonic(sonic);
}

void loop() {
  buggy->update();
  sonic->ultraLoop();
  christmasTree->update();
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
