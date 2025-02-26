using UnityEngine;

namespace DefaultNamespace
{
	
	[RequireComponent(typeof(PositionSaver))]
	public class EditorMover : MonoBehaviour
	{
		private PositionSaver _save;
		private float _currentDelay;

		//todo comment: Что произойдёт, если _delay > _duration?
		//Не сохранится ни одной позиции.
		[SerializeField, Range(0.2f, 1.0f)]
		private float _delay = 0.5f;
		[SerializeField, Min(0.2f)]
		private float _duration = 5f;

		private void Start()
		{
			if(_duration < _delay)
			{
				_duration = 5 * _delay;
			}
			//todo comment: Почему этот поиск производится здесь, а не в начале метода Update?
			//Если вызывать его в Update, то при каждом обновлении кадра список будет очищаться
			_save = GetComponent<PositionSaver>();
			_save.Records.Clear();
		}

		private void Update()
		{
			_duration -= Time.deltaTime;
			if (_duration <= 0f)
			{
				enabled = false;
				Debug.Log($"<b>{name}</b> finished", this);
				return;
			}
			
			//todo comment: Почему не написать (_delay -= Time.deltaTime;) по аналогии с полем _duration?
			//deltaTime может отличаться в зависимости от fps, а нам необходимо записывать свою позицию каждый _delay секунд. В случае с _duration нам не требуется такой точности.
			_currentDelay -= Time.deltaTime;
			if (_currentDelay <= 0f)
			{
				_currentDelay = _delay;
				_save.Records.Add(new PositionSaver.Data
				{
					Position = transform.position,
					//todo comment: Для чего сохраняется значение игрового времени?
					//Для дальнейшего использования в ReplayMover
					Time = Time.time,
				});
			}
		}
	}
}