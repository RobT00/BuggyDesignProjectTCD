#include "Lights.h"

constexpr int16_t Lights::loopDuration;
constexpr int16_t Lights::indicatorPeriod;
constexpr int8_t Lights::leftIndicatorPin;
constexpr int8_t Lights::rightIndicatorPin;
constexpr int8_t Lights::nPins;
constexpr int8_t Lights::pins[];

Lights::Lights() {
  for (const auto &pin : pins) {
    pinMode(pin, OUTPUT);
    off(pin);
  }
  on(leftIndicatorPin);
  on(rightIndicatorPin);
}

void Lights::setMotor(const MotorControls *motor) {
  motor_ = motor;
}

void Lights::setUltrasonic(const UltraSonic *ultra) {
  ultra_ = ultra;
}

void Lights::update() {
  if (motor_ && motor_->getState() == MotorState::STOPPED) {
    for (int8_t i = 0; i < nPins; i++) {
      on(pins[i]);
    }
    const bool obstacle = (ultra_ && ultra_->isBlockedByObstacle());
    setLightState(leftIndicatorPin, obstacle);
    setLightState(rightIndicatorPin, obstacle);
  } else {
    const int16_t progress = ((millis() % loopDuration) / (loopDuration / nPins)) % nPins;
    bool pinOn;
    for (int8_t i = 0; i < nPins; i++) {
      pinOn = (progress == i || ((progress + nPins / 2) % nPins) == i);
      setLightState(pins[i], pinOn);
    }
    if(motor_ && motor_->getState() == MotorState::RIGHT_OVERRIDE) {
      const bool indicatorOn = (millis() % indicatorPeriod) < indicatorPeriod / 2;
      setLightState(leftIndicatorPin, indicatorOn);
      off(rightIndicatorPin);
    } else if(motor_ && motor_->getState() == MotorState::LEFT_OVERRIDE) {
      const bool indicatorOn = (millis() % indicatorPeriod) < indicatorPeriod / 2;
      off(leftIndicatorPin);
      setLightState(rightIndicatorPin, indicatorOn);
    }
  }
}
