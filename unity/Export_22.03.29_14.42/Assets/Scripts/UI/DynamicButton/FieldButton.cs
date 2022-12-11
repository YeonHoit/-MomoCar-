using UnityEngine;

namespace DynamicButton
{
    public class FieldButton : MonoBehaviour
    {
        [Header("Field")]
        public string fieldInfo;

        /* Field를 클릭했을 때 실행되는 함수 */
        public void OnClickFieldButton()
        {
            Utility.utility.LoadStartCoroutine(Field.field.LoadField(fieldInfo));
        }
    }
}