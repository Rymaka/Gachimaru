using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HPBarManager : MonoBehaviour
{
    [SerializeField] private Image _currentHealthbar;
    [SerializeField] private TextMeshProUGUI  _ratioText;
    
    [SerializeField] private float _hitpoint = 150;
    [SerializeField] private float _maxHitPoint = 150;

    private void Start()
    {
        UpdateHpBar();
    }

    private void UpdateHpBar()
    {
        if (_currentHealthbar == null || _ratioText == null) return;
        
        float ratio = _hitpoint / _maxHitPoint;
        _currentHealthbar.rectTransform.localScale = new Vector3(ratio, 1, 1);
        _ratioText.text = (ratio * 100).ToString("0") + "%";
    }

    private void TakeDamage(float damage)
    {
        _hitpoint -= damage;
        if (_hitpoint <= 0)
        {
            _hitpoint = 0;
            Debug.Log("Dead");
        }
        UpdateHpBar();

    }

    private void HealDamage(float heal)
    {
        _hitpoint += heal;
        if (_hitpoint > _maxHitPoint)
        {
            _hitpoint = _maxHitPoint;
            Debug.Log("FullHP");

        }
        UpdateHpBar();

    }
}
