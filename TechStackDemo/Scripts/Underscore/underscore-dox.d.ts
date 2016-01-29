//interface Underscore<MapStringTo<T>> {



//	/**
//	* Wrapped type `object`.
//	* @see _.values
//	**/
//    values(): T[];
//}


interface UnderscoreStatic {

    extend<T,A>(
        destination: T,
        sourceA: A
    ): T & A;
    extend<T, A, B>(
        destination: T,
        sourceA: A,
        sourceB: B
    ): T & A & B;
    extends<T, A, B, C>(
        destination: T,
        sourceA: A,
        sourceB: B,
        sourceC: C
    ): T & A & B & C;
}
