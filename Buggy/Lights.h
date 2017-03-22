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

    static constexpr int16_t loopDuration = 500;
    static constexpr int16_t indicatorPeriod = 800;
    static constexpr int8_t leftIndicatorPin = 12;
    static constexpr int8_t rightIndicatorPin = 13;
    static constexpr int8_t pins[] = {5, 6, 7, 8, 9, 10, 11};
    static constexpr int8_t nPins = sizeof(Lights::pins) / sizeof(Lights::pins[0]);

    // May have to use non-intuitive polarities if using an NPN emitter follower
    void setLightState(uint8_t pin, bool on) const { digitalWrite(pin, on ? LOW : HIGH); }
    void on(uint8_t pin) const { setLightState(pin, true); }
    void off(uint8_t pin) const { setLightState(pin, false); }
};
