using UnityEngine;

namespace DynamicButton
{
    public class FieldButton : MonoBehaviour
    {
        [Header("Field")]
        public string fieldInfo;

        /* Field�� Ŭ������ �� ����Ǵ� �Լ� */
        public void OnClickFieldButton()
        {
            Utility.utility.LoadStartCoroutine(Field.field.LoadField(fieldInfo));
        }
    }
}