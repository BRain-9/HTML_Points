using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUI_Item {
	void OnOpen();
	void Close();
	void Selected();
	void Deselected();
}
