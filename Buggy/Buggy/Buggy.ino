//hardware interface
#define CTRL_PIN 3
#define IR_PIN 2
String command;
//runs once
void setup() {
  // set pins, input.output:
  pinMode(CTRL_PIN, OUTPUT);
  digitalWrite(CTRL_PIN, LOW); //set LOW
  pinMode(IR_PIN, INPUT);

  //set up serial comms
  Serial.begin(9600);
  Serial.print("+++");
  delay(1500);
  Serial.println("ATID 6968, CH C, CN");
  delay(1100);
  while(Serial.read() != -1){};
  
}

void loop() {
  // put your main code here, to run repeatedly:
  if(Serial.available() > 0){
    
  }
}


void writeXbee(String msg){
  Serial.println(msg);
  Serial.flush();
}

bool string_equals(const char* a, const char* b){
  for (short i = 0; ; i++){
    if (a[i] != b[i]){
      return false;   
    }
    if (a[i] == '\0'){
      return true;
    }
  }
}

void process_command(){
  if (command.equals("PING")) {
    writeXbee("PONG");
  }
  else if (command.equals("PONG")) {
    writeXbee("PING");
  }
  else writeXbee("NOT MESSAGE");
}

void serialEvent(){
  while (Serial.available()) {
    char message = Serial.read();
    if (message == '\n'){
      process_command();
      command = "";
    } else {
      command += message;
    }
//    if (message == '1'){
//      writeXbee("ONE");
//    } else if (message == '2'){
//      writeXbee("TWO");
//    } else {
//      writeXbee("NOT MESSAGE!");
//    }
  }
}

