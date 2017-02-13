#pragma once

#include "CommTrans.h"
#include "MotorControls.h"

#include <Arduino.h>

class Buggy {
    private:
        const CommTrans comms;
        const MotorControls motor;

        bool isGoing = false;

        unsigned long travelledTime = 0l;
        unsigned long lastGoTime = 0l;

    public:
        Buggy() = delete;
        Buggy(CommTrans *c) : comms(comms) {
            pinMode(LED_BUILTIN, OUTPUT);
        };

        void go();
        void stop();

        void flashLED() const;

        unsigned long getTravelledTime() const;
};
