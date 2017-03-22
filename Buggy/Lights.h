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

    static const int16_t loopDuration;
    static const int16_t indicatorPeriod;
    static const int8_t leftIndicatorPin;
    static const int8_t rightIndicatorPin;
    static const int8_t nPins;
    static const int8_t pins[];

    // May have to use non-intuitive polarities if using an NPN emitter follower
    void setLightState(uint8_t pin, bool on) const { digitalWrite(pin, on ? LOW : HIGH); }
    void on(uint8_t pin) const { setLightState(pin, true); }
    void off(uint8_t pin) const { setLightState(pin, false); }
};
