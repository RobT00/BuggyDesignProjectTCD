#pragma once

#include "CommTrans.h"
#include "MotorControls.h"

#include <Arduino.h>

enum Direction {
  CLOCKWISE,
  ANTI_CLOCKWISE
};

class Buggy {
 private:
    enum ParkingState {
      NOT_PARKING,
      BEFORE_INTERSECTION,
      AFTER_INTERSECTION
    };
    ParkingState parkingState = NOT_PARKING;
    const unsigned int parking_overrideOffAt = 3000;
    const unsigned int parking_stopAt = 7000;

    const CommTrans *comms;
    MotorControls motor;

    bool going = false;
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

    static const short IR_PIN = 2;
    static const short LED_PIN = 13;
  
    Buggy() = delete;
    Buggy(CommTrans *c) : comms(c) {
      pinMode(LED_PIN, OUTPUT);
      pinMode(IR_PIN, INPUT);
      motor.stop();
    };

    void go(bool silent = false);
    void stop(bool silent = false);
    void park();
    bool isGoing() const;

    Direction getDirection() const { return travelDirection; }
    void setDirection(Direction d) { travelDirection = d; }

    void flashLED() const;

    void update();
    void gantry_ISR();

    unsigned long getTravelledTime() const;
};
