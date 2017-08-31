using System;
using Gtk;
using System.Drawing;
using Gdk;

public partial class MainWindow: Gtk.Window
{
	public enum Estado
	{
		Idle,		//Em espera, Aguardando comando
		Desenhando	//Desenhando com a ferramenta selecionada
	}
	public enum Ferramenta
	{
		Nenhuma,
		Pencil,		//Lapis selecionado
		Rectangle,	//Retangulo selecionado
		Circle		//Circulo selecionado
	}
	public Estado estado_atual;
	public Ferramenta ferramenta_atual;
	public int desenhar;
	public Graphics graphic_area;
	public Pen pincel;
	public PointF pontoAnterior;
	public PointF primeiraPosicao;
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		pincel = new Pen (System.Drawing.Color.Black, 1.0f);
		graphic_area = Gtk.DotNet.Graphics.FromDrawable (drawingareaDesenho.GdkWindow);
		drawingareaDesenho.AddEvents((int) 
				(EventMask.ButtonPressMask    
				|EventMask.ButtonReleaseMask    
				|EventMask.KeyPressMask    
				|EventMask.PointerMotionMask));
		//labelStatusReal.Text = "Iniciado. Selecione a ferramenta.";
		estado_atual = Estado.Idle;
		ferramenta_atual = Ferramenta.Nenhuma;

		pontoAnterior = new PointF (0.0f, 0.0f);
		primeiraPosicao = new PointF (0.0f, 0.0f);

		labelStatusReal.Text = "Start";

		graphic_area.Clear (System.Drawing.Color.White);
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
	protected void OnButtonReleaseEvent (object sender, ButtonReleaseEventArgs a)
	{
		labelStatusReal.Text = "Aguardando.";
		estado_atual = Estado.Idle;
	}

	protected void OnButtonLapisClicked (object sender, EventArgs e)
	{
		labelStatusReal.Text = "Lápis Selecionado.";
		ferramenta_atual = Ferramenta.Pencil;
	}

	protected void OnButtonRetanguloClicked (object sender, EventArgs e)
	{
		labelStatusReal.Text = "Retângulo Selecionado.";
		ferramenta_atual = Ferramenta.Rectangle;
	}

	protected void OnButtonCirculoClicked (object sender, EventArgs e)
	{
		labelStatusReal.Text = "Círculo Selecionado.";
		ferramenta_atual = Ferramenta.Circle;
	}

	protected void OnDrawingareaDesenhoButtonPressEvent (object o, ButtonPressEventArgs args)
	{
		labelStatusReal.Text = "Desenhando.";
		estado_atual = Estado.Desenhando;
		primeiraPosicao.X = (float)(args.Event.X - drawingareaDesenho.Allocation.X);
		primeiraPosicao.Y = (float)(args.Event.Y- drawingareaDesenho.Allocation.Y);
		pontoAnterior = primeiraPosicao;
	}

	protected void OnDrawingareaDesenhoButtonReleaseEvent (object o, ButtonReleaseEventArgs args)
	{
		//labelStatusReal.Text = "Aguardando.";
		estado_atual = Estado.Idle;
	}

	protected void OnDrawingareaDesenhoMotionNotifyEvent (object o, MotionNotifyEventArgs args)
	{
		PointF pontoAtual;
		
		switch (estado_atual) {
		case Estado.Idle:

			break;
		case Estado.Desenhando:
			switch (ferramenta_atual) {
			case Ferramenta.Pencil:
				pontoAtual = new PointF (
					(float)(args.Event.X - drawingareaDesenho.Allocation.X),
					(float)(args.Event.Y - drawingareaDesenho.Allocation.Y));
//				graphic_area.DrawEllipse (p,
//					(int)(args.Event.X - drawingareaDesenho.Allocation.X),
//					(int)args.Event.Y,
//					3.0f, 3.0f);
//				graphic_area.FillEllipse (
//					pincel.Brush,
//					pontoAtual.X,
//					pontoAtual.Y,
//					pincel.Width,
//					pincel.Width);
				graphic_area.DrawLine (
					pincel,
					pontoAnterior,
					pontoAtual);
				pontoAnterior = pontoAtual;
				break;
			case Ferramenta.Rectangle:
				pontoAtual = new PointF (
					(float)(args.Event.X - drawingareaDesenho.Allocation.X),
					(float)(args.Event.Y - drawingareaDesenho.Allocation.Y));

				graphic_area.FillRectangle (
					new Pen (System.Drawing.Color.White, pincel.Width).Brush,
					primeiraPosicao.X,
					primeiraPosicao.Y,
					pontoAnterior.X - primeiraPosicao.X,
					pontoAnterior.Y - primeiraPosicao.Y);

//				graphic_area.DrawRectangle (
//					pincel,
//					primeiraPosicao.X,
//					primeiraPosicao.Y,
//					pontoAtual.X - primeiraPosicao.X,
//					pontoAtual.Y - primeiraPosicao.Y);

				graphic_area.FillRectangle (
					pincel.Brush,
					primeiraPosicao.X,
					primeiraPosicao.Y,
					pontoAtual.X - primeiraPosicao.X,
					pontoAtual.Y - primeiraPosicao.Y
				);
				pontoAnterior = pontoAtual;
				break;
			case Ferramenta.Circle:
				pontoAtual = new PointF (
					(float)(args.Event.X - drawingareaDesenho.Allocation.X),
					(float)(args.Event.Y - drawingareaDesenho.Allocation.Y));

				graphic_area.FillEllipse (
					new Pen (System.Drawing.Color.White, pincel.Width).Brush,
					primeiraPosicao.X,
					primeiraPosicao.Y,
					pontoAnterior.X - primeiraPosicao.X,
					pontoAnterior.Y - primeiraPosicao.Y);

				//				graphic_area.DrawRectangle (
				//					pincel,
				//					primeiraPosicao.X,
				//					primeiraPosicao.Y,
				//					pontoAtual.X - primeiraPosicao.X,
				//					pontoAtual.Y - primeiraPosicao.Y);

				graphic_area.FillEllipse(
					pincel.Brush,
					primeiraPosicao.X,
					primeiraPosicao.Y,
					pontoAtual.X - primeiraPosicao.X,
					pontoAtual.Y - primeiraPosicao.Y
				);
				pontoAnterior = pontoAtual;
				break;
			case Ferramenta.Nenhuma:
				labelStatusReal.Text = "Selecione uma Ferramenta.";
				break;
			default:

				break;
			}
			break;
		}

		//labelStatusReal.Text = "Motion " + args.Event.X.ToString() + ", " +  args.Event.Y.ToString();
	}

	protected void OnColorbuttonPincelColorSet (object sender, EventArgs e)
	{
		System.Drawing.Color new_color;
		new_color = System.Drawing.Color.FromArgb (
			(colorbuttonPincel.Color.Red * 255)/65535,
			(colorbuttonPincel.Color.Green * 255)/65535,
			(colorbuttonPincel.Color.Blue * 255)/65535);
		pincel.Color = new_color;

	}

	protected void OnHscaleEspessuraValueChanged (object sender, EventArgs e)
	{
		pincel.Width = (float) hscaleEspessura.Value;
	}

	protected void OnButtonClearClicked (object sender, EventArgs e)
	{
		labelStatusReal.Text = "Tela Limpa.";
		graphic_area.Clear (System.Drawing.Color.White);
	}
		
		
}
