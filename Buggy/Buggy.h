#pragma once

#include "CommTrans.h"
#include "MotorControls.h"

#include <Arduino.h>

class Buggy {
  private:
    enum ParkingState {
      NOT_PARKING,
      BEFORE_INTERSECTION,
      IN_INTERSECTION,
      AFTER_INTERSECTION
    };
    ParkingState parkingState = NOT_PARKING;
    const unsigned int parking_overrideAt = 2000;
    const unsigned int parking_overrideOffAt = 2500;
    const unsigned int parking_stopAt = 4500;

    const CommTrans *comms;
    const MotorControls motor;

    bool isGoing = false;
    Direction travelDirection = CLOCKWISE;
    unsigned long travelledTime = 0l;
    unsigned long lastGoTime = 0l;
 
    volatile bool irInterrupt = false;
    volatile bool underGantry = false;
    unsigned long atGantryAt = 0l;

    void detectGantry();
    void updateParking();
    int readGantry() const;
    unsigned long timeTravelledSinceGantry() const;

  public:
    enum Direction {
        CLOCKWISE,
        ANTI_CLOCKWISE
    };
    static const short IR_PIN = 2;
    static const short LED_PIN = 13;
  
    Buggy() = delete;
    Buggy(CommTrans *c) : comms(c) {
      pinMode(LED_PIN, OUTPUT);
      pinMode(IR_PIN, INPUT);
    };

    void go(bool silent = false);
    void stop(bool silent = false);
    void park();

    void flashLED() const;

    void update();
    void gantry_ISR();

    unsigned long getTravelledTime() const;
};
