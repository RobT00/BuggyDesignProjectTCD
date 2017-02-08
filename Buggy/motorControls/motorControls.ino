void setup() {
  // put your setup code here, to run once:

}

void loop() {
  // put your main code here, to run repeatedly:

}


class motorControl {

#define bcc_pin 1;



 
motorControl() {
pinMode(bcc_pin, OUTPUT);
digitalWrite(bcc_pin, LOW);

}

void leftOveride () {
  digitalWrite(bcc_pin, HIGH);
  delay(6);
  digitalWrite(bcc_pin, LOW);
  }

void rightOveride () {
  digitalWrite(bcc_pin, HIGH);
  delay(8);
  digitalWrite(bcc_pin, LOW);
  }

void go () {
  digitalWrite(bcc_pin, HIGH);
  delay(4);
  digitalWrite(bcc_pin, LOW);
  }

void stopBuggy () {
  digitalWrite(bcc_pin, HIGH);
  delay(2);
  digitalWrite(bcc_pin, LOW);
  }    


};
