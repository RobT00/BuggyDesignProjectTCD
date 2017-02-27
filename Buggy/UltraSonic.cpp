#include "UltraSonic.h"

#include "Arduino.h"

void UltraSonic::ultraLoop() {
  unsigned long currentMillis = millis();
  if (currentMillis - lastPingTime >= pingInterval) {
    lastPingTime = currentMillis;
    unsigned long distance = measureDistance();
    // Serial.print("Value: ");
    // Serial.println(value);
    if (obstacle == false && distance < tresholdDistance) {
      buggy->stop(true);
      comms->writeXbee("OBSTACLE");
      obstacle = true;
    } else if (obstacle == true && distance > tresholdDistance) {
      buggy->go(true);
      comms->writeXbee("PATHCLEAR");
      obstacle = false;
    }
  }
}

unsigned long UltraSonic::measureDistance() const {
  // Request ultrasonic pulse
  pinMode(ultraPin, OUTPUT);
  digitalWrite(ultraPin, LOW);
  digitalWrite(ultraPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(ultraPin, LOW);

  // Recieve distance data
  pinMode(ultraPin, INPUT);
  unsigned long pulseLength = 0;
  pulseLength = pulseIn(ultraPin, HIGH);

  // Calculate distance
  double distance = ((pulseLength / 2) * .03432);
  return (unsigned long) distance;
}
