namespace Chess
{
namespace Definitions
{

// represents the result of an attack after the d6 has been rolled. Mainly used 
// to package the success state of the roll and the roll itself for UI purposes
public readonly struct AttackResult {
    // what the d6 (plus any modifer) resulted in
    public readonly int roll_result;
    // whether that roll was successful, given the roll table
    public readonly bool was_successful;

    // constructor
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