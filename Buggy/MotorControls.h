#pragma once

class MotorControls {
  private:
    static const short bcc_pin = 3;

    void write(short duration) const;

  public:
    MotorControls();
    void leftOverride() const;
    void rightOverride() const;
    void go() const;
    void stop() const;
};
