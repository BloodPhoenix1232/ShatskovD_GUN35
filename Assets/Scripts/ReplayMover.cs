using UnityEngine;

namespace DefaultNamespace
{
	[RequireComponent(typeof(PositionSaver))]
	public class ReplayMover : MonoBehaviour
	{
		private PositionSaver _save;

		private int _index;
		private PositionSaver.Data _prev;
		private float _duration;

		private void Start()
		{
			////todo comment: зачем нужны эти проверки?
			///Первая проверяет компонент на NullReferecnce, а вторая предотвращает работу с пустым списком.
			if (!TryGetComponent(out _save) || _save.Records.Count == 0)
			{
				Debug.LogError("Records incorrect value", this);
				//todo comment: Для чего выключается этот компонент?
				//Чтобы дальнейший код не расспространялся на текущий объект
				enabled = false;
			}
		}

		private void Update()
		{
			var curr = _save.Records[_index];
			//todo comment: Что проверяет это условие (с какой целью)? 
			//Прошло ли время до времени сохраненного в списке, что-то типо задержки или ожидания.
			if (Time.time > curr.Time)
			{
				_prev = curr;
				_index++;
				//todo comment: Для чего нужна эта проверка?
				//Чтобы не выйти за пределы списка и не вызвать ошибку
				if (_index >= _save.Records.Count)
				{
					enabled = false;
					Debug.Log($"<b>{name}</b> finished", this);
				}
			}
			//todo comment: Для чего производятся эти вычисления (как в дальнейшем они применяются)?
			//В методе Lerp, дельта - это часть времени, которое прошло между кадрами. Чем меньше дельта, тем более плавное будет перемещение
			var delta = (Time.time - _prev.Time) / (curr.Time - _prev.Time);
			//todo comment: Зачем нужна эта проверка?
			//Чтобы избежать ошибок, так как при передаче Nan в функцию Lerp поломает метод.
			if (float.IsNaN(delta)) delta = 0f;
			//todo comment: Опишите, что происходит в этой строчке так подробно, насколько это возможно
			//Объект на котором наложен этот компонент будет изменять свою позицию. В данном случае применяется линейная интерполяция, то есть объект будет смещаться линейно из _prev в curr. Коэффициент дельта показывает какой путь должен преодолеть объект из a в b(условно 0.25 - это четверть пути).
			transform.position = Vector3.Lerp(_prev.Position, curr.Position, delta);
		}
	}
}