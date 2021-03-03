namespace Chess
{
namespace Definitions
{

public readonly struct AttackResult {
    public readonly int roll_result;
    public readonly bool was_successful;

    public AttackResult(
        int roll_result,
        bool was_successful
    ) {
        this.roll_result = roll_result;
        this.was_successful = was_successful;
    }
        
}

} // Definitions
} // Chess