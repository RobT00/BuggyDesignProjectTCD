#include "HashMap.h"
#include "CommTrans.h"

CommTrans comm1;

void setup() {
  // put your setup code here, to run once:

  comm1.init();

}

void loop() {
  // put your main code here, to run repeatedly:

}

void serialEvent(){
  while (Serial.available()) {
    char message = Serial.read();
    process_command(message);
  }
}

