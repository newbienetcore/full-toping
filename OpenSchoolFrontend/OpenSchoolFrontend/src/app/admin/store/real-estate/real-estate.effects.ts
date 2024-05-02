import { Injectable } from "@angular/core";
import { of } from 'rxjs';
import { catchError, map, mergeMap, tap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { CrudService } from "src/app/admin/services/crud.service";
import { fetchfeedbackdataData, fetchfeedbackdataFailure, fetchfeedbackdataSuccess, fetchpropertydataData, fetchpropertydataSuccess, fetchrentproprtydataData, fetchrentproprtydataSuccess, fetchsalepropertydataData, fetchsalepropertydataFailure, fetchsalepropertydataSuccess } from "./real-estate.action";
import { fetchproductsFailure } from "../ecommerce/ecommerce.actions";
import { fetchrecentcourseFailure, fetchrecentcourseSuccess } from "../learning/learning.action";


@Injectable()
export class RealEffects {
    fetchData$ = createEffect(() =>
        this.actions$.pipe(
            ofType(fetchpropertydataData),
            mergeMap(() =>
                this.CrudService.fetchData('/app/property').pipe(
                    map((propertydata) => fetchpropertydataSuccess({ propertydata })),

                    catchError((error) =>
                        of(fetchproductsFailure({ error }))
                    )

                )
            )
        )
    );

    fetchrecentData$ = createEffect(() =>
        this.actions$.pipe(
            ofType(fetchfeedbackdataData),
            mergeMap(() =>
                this.CrudService.fetchData('/app/feedback').pipe(
                    map((feedbackdata) => fetchfeedbackdataSuccess({ feedbackdata })),

                    catchError((error) =>
                        of(fetchfeedbackdataFailure({ error }))
                    )

                )
            )
        )
    );
    fetchsalepropertyData$ = createEffect(() =>
        this.actions$.pipe(
            ofType(fetchsalepropertydataData),
            mergeMap(() =>
                this.CrudService.fetchData('/app/saleproperty').pipe(
                    map((salepropertydata) => fetchsalepropertydataSuccess({ salepropertydata })),

                    catchError((error) =>
                        of(fetchsalepropertydataFailure({ error }))
                    )

                )
            )
        )
    );
    fetchrecentcoursetData$ = createEffect(() =>
        this.actions$.pipe(
            ofType(fetchrentproprtydataData),
            mergeMap(() =>
                this.CrudService.fetchData('/app/rentproperty').pipe(
                    map((rentproprtydata) => fetchrentproprtydataSuccess({ rentproprtydata })),

                    catchError((error) =>
                        of(fetchrecentcourseFailure({ error }))
                    )

                )
            )
        )
    );

    constructor(
        private actions$: Actions,
        private CrudService: CrudService
    ) { }
} 