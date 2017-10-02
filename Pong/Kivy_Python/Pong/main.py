from kivy.app import App
from kivy.uix.widget import Widget
from kivy.properties import ObjectProperty
from kivy.properties import NumericProperty, ReferenceListProperty
from kivy.vector import Vector
from kivy.clock import Clock
from random import randint


class PongBola(Widget):

    # velocity of the ball on x and y axis
    velocity_x = NumericProperty(0)
    velocity_y = NumericProperty(0)
    velocity = ReferenceListProperty(velocity_x, velocity_y)

    def move(self):
        self.pos = Vector(*self.velocity) + self.pos

class PongGoleiro(Widget):

    score = NumericProperty(0)

    def bounce_ball(self, ball):
        if self.collide_widget(ball):
            vx, vy = ball.velocity
            bounced = Vector(-1 * vx, vy)
            vel = bounced * 1.1
            ball.velocity = vel.x, vel.y


class PongGame(Widget):
    ball = ObjectProperty(None)
    player1 = ObjectProperty(None)
    player2 = ObjectProperty(None)
    score1 = ObjectProperty(None)
    score2 = ObjectProperty(None)

    def update(self, dt):
        self.ball.move()

        # bounce goleiros
        self.player1.bounce_ball(self.ball)
        self.player2.bounce_ball(self.ball)

        # bounce off top and bottom
        if (self.ball.y < 10) or (self.ball.top > self.height - 10):
            self.ball.velocity_y *= -1

        # went of to a side to score point
        if self.ball.x < self.x:
            self.player2.score += 1
            self.score2.text = str(self.player2.score)
            self.serve_ball(Vector(10, 0).rotate(randint(210-90, 330-90)))
        if self.ball.x + self.ball.width > self.width:
            self.player1.score += 1
            self.score1.text = str(self.player1.score)
            self.serve_ball(Vector(10, 0).rotate(randint(30-90, 150-90)))

    def serve_ball(self, vel=(4, 0)):
        self.ball.center = self.center
        self.ball.velocity = vel

    def on_touch_move(self, touch):
        if touch.x < self.width / 3:
            self.player1.center_y = touch.y
        if touch.x > self.width - self.width / 3:
            self.player2.center_y = touch.y

class PongApp(App):

    def build(self):
        game = PongGame()
        game.serve_ball(Vector(10, 0).rotate(randint(0, 360)))
        Clock.schedule_interval(game.update, 1.0 / 30.0)
        return game


if __name__ == '__main__':
    PongApp().run()