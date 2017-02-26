#include "CommTrans.h"

#include "Arduino.h"

void CommTrans::writeXbee(String command) const {
  Serial.print(my_ID);
  Serial.print(" ");
  Serial.print(0);
  Serial.print(" ");
  Serial.print(command);
  Serial.print('\n');
  Serial.flush();
}

void CommTrans::processCommand(char c) {
  if (c != '\n') {
    message += c;
    return;
  }

  if (message.length() < 5) {
    message = "";
    return;
  }

  short send_ID = message[0] - '0';
  short reci_ID = message[2] - '0';
  String command = message.substring(4);
  message = "";

  if (send_ID != 0) {
    return;
  }
  if (reci_ID != my_ID) {
    return;
  }

  VoidFunction f = handlers.get(command);
  if (f != NULL) {
    f();
  } else if (defaultHandler != NULL) {
    defaultHandler();
  }
}

void CommTrans::init() const {
  Serial.begin(9600);
  Serial.print("+++");
  delay(1500);
  Serial.println("ATID 6968, CH C, CN");
  delay(11000);
  while (Serial.read() != -1) {}
}

void CommTrans::addHandler(String command, VoidFunction handler) {
  handlers.add(command, handler);
}

void CommTrans::setDefaultHandler(VoidFunction handler) {
  defaultHandler = handler;
}
