#pragma once

#include "CommTrans.h"
#include "MotorControls.h"

#include <Arduino.h>

class Buggy {
    private:
        const CommTrans comms;
        const MotorControls motor;

        bool isGoing = false;
        bool atGantry = false;
        bool irInterrupt = false;

        unsigned long travelledTime = 0l;
        unsigned long lastGoTime = 0l;

        int readGantry() const;

    public:
        static const short IR_PIN = 2;
    
        Buggy() = delete;
        Buggy(CommTrans *c) : comms(comms) {
            pinMode(LED_BUILTIN, OUTPUT);
        };

        void go();
        void stop1();

        void flashLED() const;

        void gantry_ISR();
        void detectGantry();
        
        unsigned long getTravelledTime() const;
};
