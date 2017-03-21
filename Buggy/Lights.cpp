#include "Lights.h"

const int16_t Lights::loopDuration = 500;
const int16_t Lights::indicatorPeriod = 800;
const int8_t Lights::leftIndicatorPin = 12;
const int8_t Lights::rightIndicatorPin = 13;
const int8_t Lights::pins[] = {5, 6, 7, 8, 9, 10, 11};
const int8_t Lights::nPins = sizeof(Lights::pins) / sizeof(Lights::pins[0]);

Lights::Lights() {
  for (auto &pin : pins) {
    pinMode(pin, OUTPUT);
    digitalWrite(pin, LOW);
  }
  digitalWrite(leftIndicatorPin, HIGH);
  digitalWrite(rightIndicatorPin, HIGH);
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
      digitalWrite(pins[i], HIGH);
    }
    const uint8_t indicatorsOn = (ultra_ && ultra_->isBlockedByObstacle()) ? HIGH : LOW;
    digitalWrite(leftIndicatorPin, indicatorsOn);
    digitalWrite(rightIndicatorPin, indicatorsOn);
  } else {
    const int16_t progress = ((millis() % loopDuration) / (loopDuration / nPins)) % nPins;
    for (int8_t i = 0; i < nPins; i++) {
      if (progress == i || ((progress + nPins / 2) % nPins) == i) {
        digitalWrite(pins[i], HIGH);
      } else {
        digitalWrite(pins[i], LOW);
      }
    }
    if(motor_ && motor_->getState() == MotorState::RIGHT_OVERRIDE) {
      const uint8_t indicatorOn = ((millis() % indicatorPeriod) < indicatorPeriod / 2) ? HIGH : LOW;
      digitalWrite(rightIndicatorPin, indicatorOn);
      digitalWrite(leftIndicatorPin, LOW);
    } else if(motor_ && motor_->getState() == MotorState::LEFT_OVERRIDE) {
      const uint8_t indicatorOn = ((millis() % indicatorPeriod) < indicatorPeriod / 2) ? HIGH : LOW;
      digitalWrite(rightIndicatorPin, LOW);
      digitalWrite(leftIndicatorPin, indicatorOn);
    }
  }
}
