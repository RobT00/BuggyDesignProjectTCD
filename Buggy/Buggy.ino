#include "CommTrans.h"
#include "MotorControls.h"

CommTrans comm;

void flashLED();

void setup() {
  pinMode(LED_BUILTIN, OUTPUT);

  comm.init();
  comm.setDefaultHandler([] { comm.writeXbee("INVALID"); });
  comm.addHandler("PING", [] { comm.writeXbee("PONG"); });
  comm.addHandler("PONG", [] { comm.writeXbee("PING"); });
  comm.addHandler("LED", [] { flashLED(); });
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

void flashLED() {
  digitalWrite(LED_BUILTIN, HIGH);
  delay(300);
  digitalWrite(LED_BUILTIN, LOW);
}
