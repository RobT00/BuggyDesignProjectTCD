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

        int readGantry() const;
        unsigned long timeTravelledSinceGantry() const;

    public:
        static const short IR_PIN = 2;
    
        Buggy() = delete;
        Buggy(CommTrans *c) : comms(c) {
            pinMode(IR_PIN, INPUT);
            pinMode(LED_BUILTIN, OUTPUT);
        };

        void go();
        void stop();

        void flashLED() const;

        void gantry_ISR();
        void detectGantry();
        
        unsigned long getTravelledTime() const;
};
