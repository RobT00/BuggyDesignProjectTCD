#pragma once

#include "CommTrans.h"
#include "MotorControls.h"

#include <Arduino.h>

class Buggy {
  private:
    const CommTrans *comms;
    const MotorControls motor;

    bool isGoing = false;
    unsigned long travelledTime = 0l;
    unsigned long lastGoTime = 0l;
 
    volatile bool irInterrupt = false;
    volatile bool underGantry = false;
    unsigned long atGantryAt = 0l;

    void detectGantry();
    int readGantry() const;
    unsigned long timeTravelledSinceGantry() const;

  public:
    static const short IR_PIN = 2;
    static const short LED_PIN = 13;
  
    Buggy() = delete;
    Buggy(CommTrans *c) : comms(c) {
      pinMode(LED_PIN, OUTPUT);
      pinMode(IR_PIN, INPUT);
    };

    void go();
    void stop();

    void flashLED() const;

    void update();
    void gantry_ISR();

    unsigned long getTravelledTime() const;
};
