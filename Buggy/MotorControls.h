#pragma once

class MotorControls {
  private:
    static const short bcc_pin = 1; // TODO: Change to actual pin number

  public:
    MotorControls();
    void leftOverride();
    void rightOverride();
    void go();
    void stop();
};
