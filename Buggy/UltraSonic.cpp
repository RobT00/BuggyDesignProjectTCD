#pragma once

#include "Buggy.h"
#include "CommTrans.h"
#include "HashMap.h"
#include "MotorControls.h"
#include "UltraSonic.h"

unsigned long UltraSonic::UltraLoop(){
  
  unsigned long pulse = 0;
  pinMode(signalPin, OUTPUT);
  digitalWrite(signalPin,LOW);
  digitalWrite(signalPin,HIGH);
  delayMicroseconds(10);
  digitalWrite(signalPin,LOW);

  pinMode(signalPin, INPUT);
    
  pulse = pulseIn(signalPin, HIGH);
    
  double ultrasoundValue = ((pulse/2) * .03432);
  unsigned long ultrasoundValue1;
  ultrasoundValue1 = (unsigned long)ultrasoundValue;
  return ultrasoundValue1;
       






}