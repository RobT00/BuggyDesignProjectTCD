#include <Arduino.h>

#include "MotorControls.h"
#include "UltraSonic.h"

class Lights {
 public:
    Lights();
    void setMotor(const MotorControls *);
    void setUltrasonic(const UltraSonic *);
    void update();

 private:
    const MotorControls* motor_;
    const UltraSonic* ultra_;
    static const int8_t leftIndicatorPin;
    static const int8_t rightIndicatorPin;
    static const int16_t indicatorPeriod;
    static const int16_t loopDuration;
    static const int8_t nPins;
    static const int8_t pins[];
};
